using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public Transform enemy;
    public Transform enemySpawnerLocation;
    public TextMeshProUGUI textToChange_EnemyType;
    public TextMeshProUGUI textToChange_Waves;
    public TextMeshProUGUI textToChange_Money;

    public float waveCooldown = 5f;
    public float preparationTime = 10f;

    private int enemiesToSpawn;
    public int waveNumber = 1;
    public int index;

    List<string> waves = new();
    public string[] wavesSelected = new string[250]; // player input? difficulty selection? 4th dimension insanity?
    string[] waveTypes = { "Normal", "Mass", "Boss", "Farm", "Special" };


    private void Start()
    {
        ShuffleWaves();
        enemy.GetComponent<Enemy_Definition>().maxHealth = 50;
    }

    void Spawn()
    {
        Instantiate(enemy, enemySpawnerLocation.position, enemySpawnerLocation.rotation);
    }

    void ShuffleWaves()
    {
        System.Random wavePicker = new();

        for (int i = 0; i < wavesSelected.Length; i++)
        {
            index = wavePicker.Next(1, waveTypes.Length);
            waves.Add(waveTypes[index]);
        }
        wavesSelected = waves.ToArray();
    }

    void BoostEnemies()
    {
        // "Normal", "Mass", "Boss", "Farm", "Special"
        if (wavesSelected[waveNumber] == "Normal")
        {
            enemiesToSpawn = 4;
            enemy.GetComponent<Enemy_Definition>().maxHealth += Mathf.Log(7, enemy.GetComponent<Enemy_Definition>().maxHealth)/2;
        }

        if (wavesSelected[waveNumber] == "Mass")
        {
            enemiesToSpawn = 12;
            enemy.GetComponent<Enemy_Definition>().maxHealth += Mathf.Log(2, enemy.GetComponent<Enemy_Definition>().maxHealth) * 3; // bal
        }

        if (wavesSelected[waveNumber] == "Boss")
        {
            enemiesToSpawn = 1;
            enemy.GetComponent<Enemy_Definition>().maxHealth += Mathf.Log(2, enemy.GetComponent<Enemy_Definition>().maxHealth) * 3; // bal
        }

        if (wavesSelected[waveNumber] == "Farm")
        {
            enemiesToSpawn = 6;
            enemy.GetComponent<Enemy_Definition>().maxHealth += Mathf.Log(2, enemy.GetComponent<Enemy_Definition>().maxHealth) * 3; // bal
        }

        if (wavesSelected[waveNumber] == "Special")
        {
            enemiesToSpawn = 3;
            enemy.GetComponent<Enemy_Definition>().maxHealth += Mathf.Log(2, enemy.GetComponent<Enemy_Definition>().maxHealth) * 3; // bal
        }       
    }

    IEnumerator SpawnWave()
    {
        Debug.Log("Wave ¹" + waveNumber + " started");

        BoostEnemies();

        for (int i = 0; i < enemiesToSpawn; i++)
        { 
            Spawn();
            enemy.GetComponent<Enemy_Definition>().enemyType = wavesSelected[waveNumber];
            yield return new WaitForSeconds(0.5f); // waveCooldown @ enemy
        }

        if (wavesSelected[waveNumber + 1] != null)
        {
            wavesSelected.Skip(waveNumber);
            waveNumber++;
            Debug.Log("Next wave is: " + wavesSelected[waveNumber]);
        }
        else
        {
            Application.Quit();
        }

        Debug.Log("Next Wave coming in: " + waveCooldown + " seconds");
    }

    // Update is called once per frame
    void Update()
    {
        if (preparationTime <= 0f)
        {
            StartCoroutine(SpawnWave());
            preparationTime = waveCooldown + 5;
        }
        preparationTime -= Time.deltaTime;

        textToChange_EnemyType.SetText("Enemy Type: " + wavesSelected[waveNumber].ToString());
        textToChange_Money.SetText("Money: " + Player_Currency.money.ToString());
        textToChange_Waves.SetText("Wave: " + waveNumber.ToString());
    }
}
