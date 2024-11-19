using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public float speed = 5f;
    public float damage = 10f;
    public bool isFlying = false;

    private List<Vector3> path;
    private int currentWaypoint = 0;

    public void SetPath(List<Vector3> newPath)
    {
        path = newPath;
        currentWaypoint = 0;
    }


    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (path == null || currentWaypoint >= path.Count) return;

        Vector3 target = path[currentWaypoint];
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentWaypoint++;
        }
    }


    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}

