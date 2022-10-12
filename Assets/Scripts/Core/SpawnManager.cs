using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform enemy;
    public Transform spawner_location;
    public float cooldown = 5f;
    private float prep = 10f;
    private int enemiesToSpawn;
    public int waveNumber = 1;

    List<string> waves = new();
    private string[] wavesSelected = new string[250]; // player input? difficulty selection? 4th dimension insanity?
    string[] waveTypes = { "Normal", "Mass", "Boss", "Farm", "Special" };

    private void Start()
    {
        ShuffleWaves();
        enemy.GetComponent<Enemy_Definition>().maxHealth = 50;
    }

    void Spawn()
    {
        Instantiate(enemy, spawner_location.position, spawner_location.rotation);
    }

    void ShuffleWaves()
    {
        int index;
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
            yield return new WaitForSeconds(0.5f); // cooldown @ enemy
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

        Debug.Log("Next Wave coming in: " + cooldown + " seconds");
    }

    // Update is called once per frame
    void Update()
    {
        if (prep <= 0f)
        {
            StartCoroutine(SpawnWave());
            prep = cooldown + 5;
        }
        prep -= Time.deltaTime;
    }
}
