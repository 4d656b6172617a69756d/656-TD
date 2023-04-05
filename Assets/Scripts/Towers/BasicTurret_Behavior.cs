using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurret_Behavior : MonoBehaviour
{
    public GameObject ProjectileType;
    public Transform FireSpot;
    public Transform target;
    public string enemyTag = "Enemy";

    public int towerDamage = 20;
    public float baseAttackSpeed;
    public float attackSpeed = 1f;
    public float attackCooldown = 0f;
    public float bonusAttackSpeed = 0.1f;
    private int towerCost = 15;
    private float timeSinceLastAttack = 0f;
    public float towerRange = 15f;

    public bool abilityMultishotEnabled = false;
    public bool abilityBerserkEnabled = false;
    public bool abilityGoldmineEnabled = false;
    public bool abilityManaSpawnEnabled = false;

    public float rotationSpeed = 10f;
    public float turnSpeed = 12f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, towerRange);
    }

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        baseAttackSpeed = attackSpeed;
    }

    public int GetTowerCost()
    {
        return towerCost;
    }

    void Tower_Attack()
    {
        GameObject SpawnProjectiles = Instantiate(ProjectileType, FireSpot.position, FireSpot.rotation);
        Bullet_Interaction projectile = SpawnProjectiles.GetComponent<Bullet_Interaction>();

        if (abilityMultishotEnabled)
        {
            projectile.abilityMultishotEnabled = true;
        }

        if (abilityManaSpawnEnabled)
        {
            projectile.abilityManaSpawnEnabled = true;
        }

        if (abilityBerserkEnabled)
        {
            attackSpeed += bonusAttackSpeed;
            attackSpeed = Mathf.Clamp(attackSpeed, baseAttackSpeed, baseAttackSpeed * 2f);
            timeSinceLastAttack = 0f;
        }

        if (abilityGoldmineEnabled)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, towerRange);
            foreach (Collider hitCollider in hitColliders)
            {
                Enemy_Definition enemy = hitCollider.gameObject.GetComponent<Enemy_Definition>();
                if (enemy.isDead)
                {
                    enemy.totalBounty = Mathf.RoundToInt(enemy.totalBounty * 1.2f);
                }
            }
        }

        if (projectile != null)
        {
            projectile.Detection(target);
        }

        projectile.damage = towerDamage;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= towerRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void FixedUpdate()
    {
        attackCooldown -= Time.deltaTime;
        timeSinceLastAttack += Time.deltaTime;

        if (timeSinceLastAttack > 3f && attackSpeed > baseAttackSpeed)
        {
            attackSpeed -= bonusAttackSpeed;
        }

        if (target == null)
        {
            transform.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), turnSpeed * Time.deltaTime);
            if (attackCooldown <= 0f)
            {
                Tower_Attack();
                attackCooldown = 1f / attackSpeed;
            }
        }
    }
}

