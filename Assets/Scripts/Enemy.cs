using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public float speed = 5f;
    public float damage = 10f;
    public bool isFlying = false;

    private List<Vector3> path;
    private Vector3 direction;
    private int currentWaypoint = 0;
    Animator anim;

    public void Start()
    {
        float prob = Random.Range(0f, 1f);

        if(prob < .33)
        {
            speed -= 1;
        }else if(prob > .66)
        {
            speed += 1;
        }

        anim = GetComponent<Animator>();
    }

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

        anim.SetBool("Walk",true);
        Vector2Int facing = new Vector2Int((int)(Mathf.Round(direction.x*10f)*.1f),(int)(Mathf.Round(direction.z * 10f) * .1f));

        Debug.Log($"direction: {direction}, facing: {facing}");
        if (facing.Equals(new Vector2Int(0, 1))) { transform.rotation = Quaternion.Euler(0, 0, 0); }
        if (facing.Equals(new Vector2Int(1, 0))) { transform.rotation = Quaternion.Euler(0, 90, 0); }
        if (facing.Equals(new Vector2Int(0, -1))) { transform.rotation = Quaternion.Euler(0, 180, 0); }
        if (facing.Equals(new Vector2Int(-1, 0))) { transform.rotation = Quaternion.Euler(0, 270, 0); }
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

    private void OnCollisionEnter(Collision collision)
    {
        string collisionType = collision.gameObject.tag;
        if (collisionType == "projectile")
        {
            TakeDamage(1);
        }
    }

    public float GetRemainingDistance()
    {
        if (path == null || currentWaypoint >= path.Count) return 0f;

        float distance = 0f;

        for (int i = currentWaypoint; i < path.Count - 1; i++)
        {
            distance += Vector3.Distance(path[i], path[i + 1]);
        }

        distance += Vector3.Distance(transform.position, path[currentWaypoint]);

        return distance;
    }
}

