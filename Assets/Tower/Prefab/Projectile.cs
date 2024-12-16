using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject target;
    private float damage;
    private float velocity;


    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) {
            transform.Translate(
                velocity * Time.deltaTime * (target.transform.position - transform.position).normalized
            );
            if ((target.transform.position - transform.position).magnitude < 0.1)
            {
                target.GetComponent<Enemy>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else { Destroy(gameObject); }
        
    }

    public void SetupProjectile(GameObject target, float damage, float velocity)
    {
        this.damage = damage;
        this.velocity = velocity;
        this.target = target;
    }
}
