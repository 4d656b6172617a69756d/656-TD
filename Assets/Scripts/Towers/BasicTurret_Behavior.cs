using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurret_Behavior : MonoBehaviour, ITower
{
    public GameObject ProjectileType;
    public Transform FireSpot;
    public Transform target;

    public string enemyTag = "Enemy";

    public float attackSpeed = 1f;
    public float attackCooldown = 0f;
    public float rotationSpeed = 10f;
    public float turnSpeed = 12f;
    public float towerRange = 15f;

    public int towerDamage = 20;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, towerRange);
    }

    void Tower_Attack()
    {
        GameObject SpawnProjectiles = Instantiate(ProjectileType, FireSpot.position, FireSpot.rotation);
        Bullet_Interaction projectile = SpawnProjectiles.GetComponent<Bullet_Interaction>();
        projectile.damage = towerDamage;
        
        if (projectile != null)
        {
            projectile.Detection(target);
        }
    }

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
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

