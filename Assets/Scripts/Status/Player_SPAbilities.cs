using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_SPAbilities : MonoBehaviour
{
    public float Slam_Cooldown;
    public float slamCD;

    public float Slow_Cooldown;
    public float slowCD;

    public float PowerUp_Cooldown;
    public float powerupCD;

    public float Income_Cooldown;
    public float incomeCD;

    void Update()
    {
        slamCD -= Time.deltaTime;
        slowCD -= Time.deltaTime;
        powerupCD -= Time.deltaTime;
        incomeCD -= Time.deltaTime;
    }
    public void Ability_Slam(int slamDamage) // deals damage to all enemies
    {
        GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (AllEnemies.Length == 0 || slamCD >= 0)
        {
            Debug.Log("Slam Failed!");
        }
        else
        {
            slamCD = Slam_Cooldown;
            Debug.Log("Ability Slam is used on " + AllEnemies.Length);
            foreach (GameObject enemy in AllEnemies)
            {
                enemy.GetComponent<Enemy_Definition>().TakeDamage(slamDamage);
                Debug.Log("Enemy Health after Slam: " + enemy.GetComponent<Enemy_Definition>().currentHealth);
            }
        }
    }
    public void Ability_Slow(StatusEffectData _data) // slows all enemies by 50% 
    {
        GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (AllEnemies.Length == 0 || slowCD >= 0)
        {
            Debug.Log("Slow Failed!");
        }
        else
        {
            slowCD = Slow_Cooldown;
            Debug.Log("Ability Slow was used on " + AllEnemies.Length);

            foreach (GameObject enemy in AllEnemies)
            {
                enemy.GetComponent<IEffectable>().Debuff_Apply(_data);
            }
        }
    }
    public void Ability_Income(int incomePercent) // gives money after some time
    {
        StartCoroutine(IncomeManager(incomePercent));
    }

    IEnumerator IncomeManager(int value)
    {
        if (incomeCD >= 0 || Player_Currency.money < 50)
        {
            Debug.Log("Income Failed!");
        }
        else
        {
            Player_Currency.money -= 50;
            incomeCD = Income_Cooldown;

            yield return new WaitForSeconds(5);
            Player_Currency.money += ((Player_Currency.money * value) / 100) + 25;
            Debug.Log("Income received, total money: " + Player_Currency.money);
        }
    }

    public void Ability_PowerUp(float powerupDuration) // adds damage to all towers for 10 seconds
    {
        StartCoroutine(PowerUpManager(powerupDuration));
    }

    IEnumerator PowerUpManager(float duration)
    {
        GameObject[] AllTowers = GameObject.FindGameObjectsWithTag("Tower");
        if (AllTowers.Length == 0)
        {
            Debug.Log("Powerup Failed!");
        }
        else
        {
            powerupCD = PowerUp_Cooldown;

            foreach (GameObject tower in AllTowers)
            {
                tower.GetComponent<BasicTurret_Behavior>().towerDamage += 50;
                Debug.Log("Powerup active, tower damage increased to: " + tower.GetComponent<BasicTurret_Behavior>().towerDamage);
            }

            yield return new WaitForSeconds(duration);
            foreach (GameObject tower in AllTowers)
            {
                tower.GetComponent<BasicTurret_Behavior>().towerDamage -= 50;
            }
        }
    }
}
