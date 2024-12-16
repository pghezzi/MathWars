using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject target;
    private float damage;
    private float velocity;

    private AoeTower source;

    private Boolean areaOfEffect;


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
        }
        if (!areaOfEffect) {
                if((target.transform.position - transform.position).magnitude < 0.1)
            {
                target.GetComponent<Enemy>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        else {
            if ((target.transform.position - transform.position).magnitude < 0.1) {
                Collider[] enemiesToHit = Physics.OverlapSphere(transform.position,source.surroundingDistance);
                enemiesToHit = enemiesToHit.Where(item => item.tag == target.tag).ToArray();
                Array.ForEach(enemiesToHit,curEnemy => curEnemy.GetComponent<Enemy>().TakeDamage(damage));
            }
        }
    }

    public void SetupProjectile(GameObject target, float damage, float velocity, AoeTower source = null)
    {
        this.damage = damage;
        this.velocity = velocity;
        this.target = target;
        this.source = source;
        this.areaOfEffect = source != null;
    }
}