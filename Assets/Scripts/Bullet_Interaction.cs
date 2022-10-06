using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Interaction : MonoBehaviour
{
    public GameObject onHit_Blood;
    private Transform target;
    public float speed = 70f;
    public int damage = 25;

    [SerializeField] StatusEffectData _data;

    public void Detection(Transform enemy)
    {
        target = enemy;
    }

    void BulletHit()
    {
        GameObject onHitEffect = Instantiate(onHit_Blood, transform.position, transform.rotation);
        Destroy(onHitEffect, 1f);
        DealDamage(target);
        Destroy(gameObject);
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

    // Update is called once per frame
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
