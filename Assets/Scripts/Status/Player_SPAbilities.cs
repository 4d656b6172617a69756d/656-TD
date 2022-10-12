using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_SPAbilities : MonoBehaviour
{
    // public StatusEffectData _data;

    public float Slam_Cooldown;
    public float slamCD;

    public float Slow_Cooldown;
    public float slowCD;

    public float PowerUp_Cooldown;
    public float poweupCD;

    public float Income_Cooldown;
    public float incomeCD;
   

    void Update()
    {
        slamCD -= Time.deltaTime;
        slowCD -= Time.deltaTime;
        poweupCD -= Time.deltaTime;
        incomeCD -= Time.deltaTime;
    }
    public void Ability_Slam(int amount) // deals damage to all enemies
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
                enemy.GetComponent<Enemy_Definition>().TakeDamage(amount);
                Debug.Log(enemy.GetComponent<Enemy_Definition>().currentHealth);
                //enemy.GetComponent<Enemy_Definition>().currentHealth -= 10;
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
            Debug.Log("Ability Slow is used on " + AllEnemies.Length);

            foreach (GameObject enemy in AllEnemies)
            {
                enemy.GetComponent<IEffectable>().Debuff_Apply(_data);
                
                // enemy.GetComponent<Enemy_Definition>().Debuff_Apply(_data);
            }
        }
    } 
    public void Ability_Income(int percent) // gives money after some time
    {
        
    }

    public void Ability_PowerUp() // adds damage to all towers for 10 seconds
    {
        GameObject[] AllTowers = GameObject.FindGameObjectsWithTag("Tower");
        if (AllTowers.Length == 0)
        {
            Debug.Log("Powerup Failed!");
        }
        else foreach (GameObject tower in AllTowers)
        {
            Debug.Log(tower.GetComponent<Bullet_Interaction>().damage);
            tower.GetComponent<Bullet_Interaction>().damage += 50;
            Debug.Log(tower.GetComponent<Bullet_Interaction>().damage);
        }
        
    }

}