using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject closest;
    private float damage;
    private float velocity;

    // Update is called once per frame
    void Update()
    {
        if (closest != null) {
            transform.Translate(
                velocity * (closest.transform.position - transform.position).normalized
            );
        }
    }

    public void SetupProjectile(GameObject closest, float damage, float velocity)
    {
        this.damage = damage;
        this.velocity = velocity;
        this.closest = closest;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if (collision.gameObject == closest)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
