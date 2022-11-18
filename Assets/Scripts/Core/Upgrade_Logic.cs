using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_Logic : MonoBehaviour
{
    public int upgradeTier_Spells = 1;
    public int upgradeTier_Towers = 1;
    public int upgradePrice = 100;
    public float upgradeTime = 20;
    public bool isUpgrading;

    public void UpgradeSpells()
    {
        if (!isUpgrading)
        {
            isUpgrading = true;
            Player_Currency.money -= upgradePrice;

            while (upgradeTime < 0)
            {
                upgradeTime -= Time.deltaTime;
            }
            upgradeTier_Spells++;
            upgradeTime = 20;
            upgradePrice *= 2;
            isUpgrading = false;
            
        }
        else Debug.Log("Something went wrong!");
        
    }

    public void UpgradeTowers()
    {
        if (!isUpgrading)
        {
            isUpgrading = true;
            Player_Currency.money -= upgradePrice;

            while (upgradeTime < 0)
            {
                upgradeTime -= Time.deltaTime;
            }
            upgradeTier_Towers++;
            upgradeTime = 20;
            upgradePrice *= 2;
            isUpgrading = false;
        }
        else Debug.Log("Something went wrong!");
    }
}
