using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public float speed = 5f;
    public int damage = 10;
    public bool isFlying = false;


    private List<Vector3> path;
    private Vector3 direction;
    private int currentWaypoint = 0;

    private Transform EndPoint;
    private InfoPanelManager InfoPanel;
    private WaveManager wave;
    
         
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Start()
    {
        InfoPanel = GameObject.Find("Info Panel").GetComponent<InfoPanelManager>();
        wave = GameObject.Find("WaveManager").GetComponent<WaveManager>();
        float prob = Random.Range(0f, 1f);


        Debug.Log($"Speed {speed}, Health {health} ");

        if(prob < .33)
        {
            speed -= 1;
        }else if(prob > .66)
        {
            speed += 1;
        }

        audioManager.PlaySFX(audioManager.enemySpawn);
    }

    public void SetPath(List<Vector3> newPath)
    {
        path = newPath;
        currentWaypoint = 0;
    }


    void Update()
    {
        Debug.Log($"Speed: {speed}, Health: {health}");
        MoveAlongPath();
        ReachedEnd();
    }

    void MoveAlongPath()
    {
        if (path == null || currentWaypoint >= path.Count) return;

        Vector3 target = path[currentWaypoint];
        Vector3 direction = (target - transform.position).normalized;

        Vector2Int facing = new Vector2Int((int)(Mathf.Round(direction.x*10f)*.1f),(int)(Mathf.Round(direction.z * 10f) * .1f));

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
        audioManager.PlaySFX(audioManager.enemyHit);
        if (health <= 0)
        {
            InfoPanel.gainCoins(damage);
            Die();
        }
    }

    public void Die()
    {
        wave.totalEnemies--;
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

    public void ReachedEnd()
    {
        if (EndPoint == null)
        {
            Debug.Log("No endpoint yet");
            return;
        }
        
        float endx = EndPoint.position.x;
        float endy = EndPoint.position.z;
        
        if (
            (transform.position.x <= EndPoint.position.x + 1f && transform.position.x >= EndPoint.position.x - 1f)
            &&
            (transform.position.z <= EndPoint.position.z + 1f && transform.position.z >= EndPoint.position.z - 1f)
        )
        {
            InfoPanel.loseHearts(damage);
            health = 0;
            Die();
        }
    }

    public void SetEndPoint(Transform point)
    {
        EndPoint = point;
    }

    public void IncreaseHealth(float amt)
    {
        health += amt;
        Debug.Log($"Current Health = {health}");
    }

    public float GetHealth()
    {
        return health;
    }

    public void IncreaseSpeed(float amt)
    {
        speed += amt;
        Debug.Log($"Current Speed = {speed}");
    }

    public float GetSpeed()
    {
        return speed;
    }
}

