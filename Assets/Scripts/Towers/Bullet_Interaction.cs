using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Interaction : MonoBehaviour
{
    public GameObject onHit_Blood;
    private Transform target;
    public float speed = 70f;
    public int damage;

    public bool multishotEnabled = false;
    public bool berserkEnabled = false;

    public float multishotRadius = 3f;
    public float multishotDamagePercent = 0.33f;

    [SerializeField] 
    private StatusEffectData _data;

    public void Detection(Transform enemy)
    {
        target = enemy;
    }

    void BulletHit()
    {
        GameObject onHitEffect = Instantiate(onHit_Blood, transform.position, transform.rotation);
        Destroy(onHitEffect, 1f);
        DealDamage(target);
        if (multishotEnabled)
        {
            ApplyMultishot();
        }
        Destroy(gameObject);
    }

    void ApplyMultishot()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, multishotRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy") && collider.gameObject != target.gameObject)
            {
                Transform enemy = collider.gameObject.transform;
                int partialDamage = Mathf.RoundToInt(damage * multishotDamagePercent);
                DealPartialDamage(enemy, partialDamage);
            }
        }
    }

    void DealPartialDamage(Transform enemy, int partialDamage)
    {
        Enemy_Definition e = enemy.GetComponent<Enemy_Definition>();
        if (e != null)
        {
            e.TakeDamage(partialDamage);
        }
    }

    void DealDamage(Transform enemy)
    {
        Enemy_Definition e = enemy.GetComponent<Enemy_Definition>();
        IEffectable d = enemy.GetComponent<IEffectable>();
        if (e != null)
        {
            e.TakeDamage(damage);
            if (d != null)
            {
                d.Debuff_Apply(_data);
            }
        }
    }



    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float traveltime = speed * Time.deltaTime; 
        if (direction.magnitude <= traveltime)
        {
            BulletHit();
            return;
        }
        transform.Translate(direction.normalized * traveltime, Space.World);

    }
}
