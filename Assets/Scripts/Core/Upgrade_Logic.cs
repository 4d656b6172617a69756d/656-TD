using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Upgrade_Logic : MonoBehaviour
{
    public GameObject upgradeButton;
    public int damageBonus = 10;
    public int attackSpeedBonus = 10;
    public int upgradeCost_Money;
    public int upgradeCost_Mana;
    public int upgradeLevel = 1;
    public int abilitiesIndex = 0;
    public int towerCost;
    public int upgradeCost;

    public List<GameObject> abilitiesList;

    private TextMeshProUGUI upgradeButtonText;


    private void Start()
    {
        foreach (GameObject ability in abilitiesList)
        {
            if (ability != null)
            {
                ability.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Unable to find ability with name " + ability);
            }
        }

        upgradeButtonText = GameObject.Find("UpgradeText").GetComponent<TextMeshProUGUI>();
        upgradeCost_Money = Random.Range(200, 500);
        upgradeCost_Mana = Random.Range(50, 100);
    }

    void Awake()
    {
        upgradeButton.GetComponent<Button>().onClick.AddListener(() => OnUpgradeButtonClicked());
    }

    private void Update()
    {
        upgradeButtonText.SetText("Upgrade\n ($" + GetUpgradeCost() + "g + " + upgradeCost_Mana + " m)");
    }

    public void OnUpgradeButtonClicked()
    {
        upgradeCost = upgradeCost_Money + ((upgradeLevel + 1) * Random.Range(3, 10));

        if (Player_Currency.money >= upgradeCost && Player_Currency.mana >= upgradeCost_Mana)
        {
            Player_Currency.money -= upgradeCost;
            Player_Currency.mana -= upgradeCost_Mana;
            UpgradeTowers();
        }
    }

    private int GetUpgradeCost()
    {
        return upgradeCost_Money + ((upgradeLevel + 1) * 10);
    }

    private void UpgradeTowers()
    {
        upgradeLevel++;
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            BasicTurret_Behavior towers = tower.GetComponent<BasicTurret_Behavior>();
            towers.towerDamage += Mathf.RoundToInt(towers.towerDamage * (damageBonus / 100f + (2 * upgradeLevel / 100f)));
            if (upgradeLevel % 3 == 0)
            {
                towers.attackSpeed += Mathf.RoundToInt(towers.attackSpeed * (attackSpeedBonus / 100f));
            }
        }

        if (abilitiesList.Count > 0)
        {
            int unlockIndex = Random.Range(0, abilitiesList.Count - 1);
            GameObject ability = abilitiesList[unlockIndex];
            abilitiesList.RemoveAt(unlockIndex);
            ability.SetActive(true);
        }
        else Debug.Log("No Abilities to learn!");
    }
}