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
    private Boolean areaOfEffect = false;

    private float curTime;

    private Vector3 startPos;


    void Start()
    {
        startPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (!areaOfEffect)
            {


                transform.Translate(
                    velocity * Time.deltaTime * (target.transform.position - transform.position).normalized
                );
                if ((target.transform.position - transform.position).magnitude < 0.1)
                {
                    target.GetComponent<Enemy>().TakeDamage(damage);
                    Destroy(gameObject);
                }
            }

            else
            {
                ParabolaMove();
                if ((target.transform.position - transform.position).magnitude < 0.1)
                {
                    Collider[] enemiesToHit = Physics.OverlapSphere(transform.position, AoeTower.surroundingDistance);
                    enemiesToHit = enemiesToHit.Where(item => item.gameObject.tag.Contains("Enemy")).ToArray();
                    Array.ForEach(enemiesToHit, curEnemy => curEnemy.GetComponent<Enemy>().TakeDamage(damage));
                    GameObject explosion = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    explosion.transform.localScale = new Vector3(0, 0, 0);
                    explosion.AddComponent<Explosion>();
                    explosion.AddComponent<AudioSource>();
                    Destroy(gameObject);
                }
                else if (curTime > 1.25) {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
        curTime += Time.deltaTime;
    }

    public void SetupProjectile(GameObject target, float damage, float velocity, Boolean area = false)
    {
        this.damage = damage;
        this.velocity = velocity;
        this.target = target;
        this.areaOfEffect = area;
    }

    public void ParabolaMove() {
        float xPosition = (1-curTime) * startPos.x + curTime * target.transform.position.x;
        float zPosition =  (1-curTime) * startPos.z + curTime * target.transform.position.z;
        float acceleration = 0.25f * (velocity * velocity) / (startPos.y - target.transform.position.y);
        float yPosition = -1 * acceleration * curTime * curTime + velocity * curTime + startPos.y;
        transform.position = new Vector3(xPosition,yPosition,zPosition);
    }
}