using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    public GameObject standardEnemyPrefab; // cat Enemy prefab
    public GameObject flyingEnemyPrefab;   // vulture Enemy prefab
    public GameObject heavyEnemyPrefab;    // bear Enemy prefab
    public Transform spawnPoint;           // Starting point
    public Transform endPoint;             // Target point

    public Level level;                    // Reference to the Level class
    public int levelDifficulty;

    public int enemiesPerWave = 1;     
    public float timeBetweenWaves = 5f;   // Not used since only one wave
    public float timeBetweenSpawns = 1f;

    private List<string> enemies; 

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

        if (levelDifficulty == 0 || levelDifficulty > 99)
        {
            Debug.Log("Level Difficulty out of range");
            return;
        }

        // Initialize A* Pathfinding
        pathfinding = new AStarPathfinding(level.grid);

        // Initialize Enemies
        enemies = InitializeEnemies();

        // Start spawning waves
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        currentWave++;
        Debug.Log("Wave " + currentWave + " starting!");

        foreach(string enemy in enemies) {

            SpawnEnemy(enemy);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        Debug.Log("All waves completed.");
        yield break; // Exit the coroutine after one wave
    }

    void SpawnEnemy(string prefab)
    {
        if (standardEnemyPrefab == null)
        {
            Debug.LogError("Standard enemy prefab is not assigned!");
            return;
        }

        Debug.Log("Spawning " + prefab);

        GameObject enemy;

        switch (prefab)
        {
            case "standard":
                enemy = Instantiate(standardEnemyPrefab, spawnPoint.position, Quaternion.identity);
                break;
            case "flying":
                enemy = Instantiate(flyingEnemyPrefab, spawnPoint.position, Quaternion.identity);
                break;
            default:
                enemy = Instantiate(heavyEnemyPrefab, spawnPoint.position, Quaternion.identity);
                break;
        }
        
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

    List<string> InitializeEnemies() //Initialize list of enemies to spawn based on level difficulty
    {
        List<string> enemies = new List<string>();

        int numStandard = 0;
        int numFlying = 0;
        int numHeavy = 0;
        if (levelDifficulty < 33)
        {
            numStandard = Mathf.CeilToInt(levelDifficulty / 3.3f);
        }
        if(levelDifficulty >= 66)
        {
            numStandard = 10;
            numFlying = Mathf.CeilToInt((levelDifficulty % 33) / 3.3f);
        }
        if(levelDifficulty > 66)
        {
            numStandard = 10;
            numFlying = 10;
            numHeavy = Mathf.CeilToInt((levelDifficulty % 33) / 3.3f);
        }

        Debug.Log($"Wave Contains {numStandard} cats, {numFlying} vultures, {numHeavy} bears");

        foreach(int num in new List<int> { 
            numStandard, numFlying, numHeavy
        })
        {
            
        }

        for(int i = 0; i < numStandard; i++) { enemies.Add("standard"); };
        for(int i = 0; i < numFlying; i++) { enemies.Add("flying"); };
        for(int i = 0; i < numHeavy; i++) { enemies.Add("heavy"); };

        System.Random rng = new System.Random();

        return enemies.OrderBy(_ => rng.Next()).ToList();

    }
}