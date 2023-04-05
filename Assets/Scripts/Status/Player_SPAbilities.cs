using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_SPAbilities : MonoBehaviour
{
    public float abilitySlamCooldown = 20f;
    public float abilitySlowCooldown = 20f;
    public float abilityPowerUpCooldown = 20f;
    public float abilityGoldmineCooldown = 20f;
    public GameObject abilityGoldmineEffect;
    private GameObject abilityGoldmineNode;
    private Dictionary<string, float> cooldownList = new();

    void Start()
    {
        cooldownList.Add("AbilitySlam", 0f);
        cooldownList.Add("AbilitySlow", 0f);
        cooldownList.Add("AbilityPowerUp", 0f);
        cooldownList.Add("AbilityGoldmine", 0f);
    }

    void Update()
    {
        UpdateCooldown("AbilitySlam");
        UpdateCooldown("AbilitySlow");
        UpdateCooldown("AbilityPowerUp");
        UpdateCooldown("AbilityGoldmine");
    }

    void UpdateCooldown(string abilityName)
    {
        if (cooldownList[abilityName] > 0f)
        {
            cooldownList[abilityName] -= Time.deltaTime;
        }
    }

    public void AbilitySlam(int slamDamage)
    {
        if (cooldownList["AbilitySlam"] > 0f)
        {
            Debug.Log("Ability Slam is on cooldown!");
            return;
        }

        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies.Length == 0)
        {
            Debug.Log("Slam Failed!");
        }
        else
        {
            cooldownList["AbilitySlam"] = abilitySlamCooldown;
            Debug.Log("Ability Slam is used on " + allEnemies.Length);
            foreach (GameObject enemy in allEnemies)
            {
                enemy.GetComponent<Enemy_Definition>().TakeDamage(slamDamage);
                Debug.Log("Enemy Health after Slam: " + enemy.GetComponent<Enemy_Definition>().currentHealth);
            }
        }
    }

    public void AbilitySlow(StatusEffectData data)
    {
        if (cooldownList["AbilitySlow"] > 0f)
        {
            Debug.Log("Ability Slow is on cooldown!");
            return;
        }

        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies.Length == 0)
        {
            Debug.Log("Slow Failed!");
        }
        else
        {
            cooldownList["AbilitySlow"] = abilitySlowCooldown;
            Debug.Log("Ability Slow was used on " + allEnemies.Length);

            foreach (GameObject enemy in allEnemies)
            {
                enemy.GetComponent<IEffectable>().Debuff_Apply(data);
            }
        }
    }

    public void AbilityPowerUp(float powerupDuration)
    {
        if (cooldownList["AbilityPowerUp"] > 0f)
        {
            Debug.Log("Powerup on cooldown!");
            return;
        }

        GameObject[] AllTowers = GameObject.FindGameObjectsWithTag("Tower");
        if (AllTowers.Length == 0)
        {
            Debug.Log("No towers to power up!");
            return;
        }

        cooldownList["AbilityPowerUp"] = abilityPowerUpCooldown;

        foreach (GameObject tower in AllTowers)
        {
            var allTowers = tower.GetComponent<BasicTurret_Behavior>();
            allTowers.towerDamage += 50;
       }

        StartCoroutine(PowerUpManager(powerupDuration));
    }

    IEnumerator PowerUpManager(float duration)
    {
        yield return new WaitForSeconds(duration);

        GameObject[] AllTowers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in AllTowers)
        {
            var allTowers = tower.GetComponent<BasicTurret_Behavior>();
            allTowers.towerDamage -= 50;
        }

        Debug.Log("Powerup expired!");
    }

    public void AbilityGoldmine(float duration)
    {
        if (cooldownList["AbilityGoldmine"] > 0f)
        {
            Debug.Log("Ability Goldmine is on cooldown!");
            return;
        }
        else
        {
            abilityGoldmineEffect.SetActive(true);
            StartCoroutine("GoldMineTest", duration);
        }
    }

    IEnumerator GoldMineTest(float duration)
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        cooldownList["AbilityGoldmine"] = abilityGoldmineCooldown;

        GameObject closestTower = null;
        GameObject[] AllTowers = GameObject.FindGameObjectsWithTag("Tower");
        float closestDistance = Mathf.Infinity;
        abilityGoldmineEffect.SetActive(false);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (GameObject tower in AllTowers)
        {
            float distance = Vector3.Distance(tower.transform.position, mousePos);
            if (distance < closestDistance)
            {
                closestTower = tower;
                closestDistance = distance;
            }
        }

        if (closestTower != null)
        {
            RaycastHit[] nodeSearch = Physics.RaycastAll(closestTower.transform.position, Vector3.down);
            foreach (RaycastHit hit in nodeSearch)
            {
                if (hit.collider.CompareTag("goldTargetNode"))
                {
                    abilityGoldmineNode = hit.collider.gameObject;
                    abilityGoldmineNode.GetComponent<Renderer>().material.color = Color.cyan;
                }
            }
            LeanTween.scale(closestTower, new Vector3(150f, 150f, 150f), 1);
            closestTower.GetComponent<BasicTurret_Behavior>().towerDamage += 100;
        }

        yield return new WaitForSeconds(duration);

        LeanTween.scale(closestTower, new Vector3(100f, 100f, 100f), 1);
        closestTower.GetComponent<BasicTurret_Behavior>().towerDamage -= 100;
        abilityGoldmineNode.GetComponent<Renderer>().material.color = new Color32(170, 209, 163, 255);
    }
}
