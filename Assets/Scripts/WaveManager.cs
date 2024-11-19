using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject standardEnemyPrefab; // Enemy prefab
    public Transform spawnPoint;          // Starting point
    public Transform endPoint;            // Target point
    public Level level;                   // Reference to the Level class

    public int enemiesPerWave = 1;        
    public float timeBetweenWaves = 5f;   // Not used since only one wave
    public float timeBetweenSpawns = 1f;  

    private AStarPathfinding pathfinding;
    private int currentWave = 0;

    void Start()
    {
        if (level == null)
        {
            Debug.LogError("Level reference is not assigned!");
            return;
        }

        if (level.grid == null)
        {
            Debug.LogError("Level grid is not initialized!");
            return;
        }

        // Initialize A* Pathfinding
        pathfinding = new AStarPathfinding(level.grid);

        // Start spawning waves
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        currentWave++;
        Debug.Log("Wave " + currentWave + " starting!");

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        Debug.Log("All waves completed.");
        yield break; // Exit the coroutine after one wave
    }

    void SpawnEnemy()
    {
        if (standardEnemyPrefab == null)
        {
            Debug.LogError("Standard enemy prefab is not assigned!");
            return;
        }

        GameObject enemy = Instantiate(standardEnemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        // Convert spawn and end points to grid coordinates
        int blockSize = level.block_size;
        Vector2Int start = new Vector2Int(
            Mathf.RoundToInt(spawnPoint.position.x / blockSize),
            Mathf.RoundToInt(spawnPoint.position.z / blockSize)
        );
        Vector2Int end = new Vector2Int(
            Mathf.RoundToInt(endPoint.position.x / blockSize),
            Mathf.RoundToInt(endPoint.position.z / blockSize)
        );

        // Calculate the path
        enemyScript.SetPath(CalculatePath(start, end));
    }

    List<Vector3> CalculatePath(Vector2Int start, Vector2Int end)
    {
        var path = pathfinding.FindPath(start, end, level.block_size);
        Debug.Log($"Path length from {start} to {end}: {path.Count}");
        return path;
    }
}
