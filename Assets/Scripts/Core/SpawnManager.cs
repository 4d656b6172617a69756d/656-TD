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
    public TextMeshProUGUI textToChange_Mana;

    public float waveCooldown = 5f;
    public float preparationTime = 10f;

    private int enemiesToSpawn;
    public int waveNumber = 1;
    public int index;

    public List<string> waves = new();
    public string[] wavesSelected = new string[250]; 
    public string[] waveTypes = { "Normal", "Mass", "Boss", "Farm", "Special" };

    public Enemy_Definition enemyDefinition;
    public Player_Currency playerCurrency;
    

    private void Start()
    {
        ShuffleWaves();
        enemyDefinition = enemy.GetComponent<Enemy_Definition>();
        enemyDefinition.maxHealth = 50;
        playerCurrency = FindObjectOfType<Player_Currency>();
    }

    private void Spawn()
    {
        Instantiate(enemy, enemySpawnerLocation.position, enemySpawnerLocation.rotation);
    }

    private void ShuffleWaves()
    {
        System.Random wavePicker = new();

        for (int i = 0; i < wavesSelected.Length; i++)
        {
            index = wavePicker.Next(1, waveTypes.Length);
            waves.Add(waveTypes[index]);
        }
        wavesSelected = waves.ToArray();
    }

    private void BoostEnemies()
    {
        Dictionary<string, (int enemiesToSpawn, float maxHealthBoost)> waveInfo = new Dictionary<string, (int, float)>
        {
            { "Normal", (8, 0.10f) },
            { "Mass", (20, 0.04f) },
            { "Boss", (1, 0.25f) },
            { "Farm", (6, 0.1f) },
            { "Special", (3, 0.2f) }
        };

        if (waveInfo.TryGetValue(wavesSelected[waveNumber], out var info))
        {
            enemiesToSpawn = info.enemiesToSpawn;
            enemyDefinition.maxHealth += 5 * Mathf.Log(1 + info.maxHealthBoost * waveNumber); // 150
        }
    }

    IEnumerator SpawnWave()
    {
        Debug.Log($"Wave ¹{waveNumber} started");
        BoostEnemies();

        for (int i = 0; i < enemiesToSpawn; i++)
        { 
            Spawn();
            enemyDefinition.enemyType = wavesSelected[waveNumber];
            yield return new WaitForSeconds(0.5f);
        }

        string[] remainingWaves = wavesSelected.Skip(waveNumber + 1).ToArray();
        waveNumber = (remainingWaves.Length > 0) ? waveNumber + 1 : waveNumber;
        Debug.Log($"Next wave is: {wavesSelected[waveNumber]}\nNext Wave coming in: {waveCooldown} seconds");

        if (remainingWaves.Length == 0)
        {
            Application.Quit();
        }
    }

    private void Update()
    {
        if (preparationTime <= 0f)
        {
            StartCoroutine(SpawnWave());
            preparationTime = waveCooldown + 5;
        }
        preparationTime -= Time.deltaTime;

        textToChange_EnemyType.SetText(string.Format("Enemy Type: {0}", wavesSelected[waveNumber]));
        textToChange_Money.SetText(string.Format("Money: {0}", Player_Currency.money));
        textToChange_Waves.SetText(string.Format("Wave: {0}", waveNumber));
        textToChange_Mana.SetText(string.Format("Mana: {0}", Player_Currency.mana));
    }
}
