using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;
using Newtonsoft.Json; // This needs to be installed from com.unity.nuget.newtonsoft-json in Package Manager
using System.Data;
using UnityEditor.Il2Cpp; 



public enum Tiles
{
    // I changed these enums incremented all by 1
    Unplacable = 0,
    Empty      = 1,
    Path       = 2,
    Tower      = 3
}

[Serializable]
public class LevelData
{
    public int level;
    public string name;
    public int[,] grid;
}

public class Level: MonoBehaviour
{

    public int bounds_min_x;
    public int bounds_min_z;
    public int bounds_max_x;
    public int bounds_max_z;
    public int bounds_size_x;
    public int bounds_size_z;
    public int block_size;

    public int width;
    public int length;
    public int storey_height = 2;
    public Tiles[,] grid;
    private Bounds bounds;
    private AStarPathfinding pathfinding;
    public Transform endPoint;

    //  Start is called before the first frame update
    void Start()
    {
        bounds_min_x = 0;
        bounds_min_z = 0;
        bounds_max_x = 200;
        bounds_max_z = 100;
        block_size = 10;
        bounds_size_x = bounds_max_x - bounds_min_x;
        bounds_size_z = bounds_max_z - bounds_min_z;
        width = bounds_size_x / block_size;
        length = bounds_size_z / block_size;
        grid = new Tiles[width, length]; 
        string project_directoy = Directory.GetCurrentDirectory() + "/Assets/Levels/";
        string level_name = "level1.json";
        string file_path = project_directoy + level_name;
        
        //  debug for testing, comment out in final
        Debug.Log(file_path);
        
        //  randgrid is for testing ONLY
        //  randGrid(10, 20);
        //  the function should have to deal with creating grid, will make need less globals 
        loadGridFromFile(file_path);
        width           = grid.GetLength(0);
        length          = grid.GetLength(1);
        bounds_size_x   = width  * block_size;
        bounds_size_z   = length * block_size;
        bounds_max_x    = bounds_size_x + bounds_min_x;
        bounds_max_z    = bounds_size_z + bounds_min_z;
        drawgrid();
        pathfinding = new AStarPathfinding(grid);
    }

    //
    // Summary:
    //     Randomly generates a grid using random values from enum Level 
    //
    // Parameters:
    //   rows:
    //     number of rows for grid
    //   cols:
    //     number of cols for grid
    //
    // Returns:
    //     Does not return anything but has side effect of setting values of grid

    void randGrid(int rows, int cols)
    {
        if (grid == null)
        {
            grid = new Tiles[rows, cols];
        }
        Tiles[] gen = new Tiles[] { Tiles.Unplacable, Tiles.Empty, Tiles.Path };
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = gen[UnityEngine.Random.Range(0, 3)];
            }
        }
    }

    //
    // Summary:
    //     Loads grid from a json file given by the inputted path
    //
    // Parameters:
    //   path:
    //     The file path to the json file with LevelData
    //
    // Returns:
    //     Does not return anything but has side effect of setting values of grid
    void loadGridFromFile(string path)
    {
        if (File.Exists(path))
        {
            string json_data = File.ReadAllText(path);
            if (string.IsNullOrEmpty(json_data))
            {
                throw new Exception("File is empty or could not be read");
            }
             
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(json_data);
            if (levelData == null)
            {
                throw new Exception("Failed to deserialize level data");
            }
            
            // JSON map data is stored as row order (r,c). grid is in col order (w,l)
            // transformations are used below to get the map rendered properly
            int rows = levelData.grid.GetLength(0);
            int cols = levelData.grid.GetLength(1);
            if (grid == null)
            {
                grid = new Tiles[cols, rows];
            }
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int tileValue = levelData.grid[r,c];
                    // [r,c] -> [c,r] : to change from row to col order
                    // row - 1 - r    : to flip image in correct orientation
                    grid[c, rows - 1 - r] = (Tiles) tileValue;
                }
            }
            Debug.Log("Grid successfully loaded from file");
        }
        else
        {
            throw new Exception("File not found at: " + path);
        }
    }


    //
    // Summary:
    //     Uses grid variable to generate the level
    //
    // Parameters:
    //   None
    //
    // Returns:
    //     Does not return anything but has side effect of setting values of grid

    void drawgrid()
    {
        int w = 0;
        for (float x = bounds_min_x; x < bounds_max_x; x += block_size, w++)
        {
            int l = 0;
            for (float z = bounds_min_z; z < bounds_max_z; z += block_size, l++)
            {
                if ((w >= width) || (l >= length))
                    continue;
                float y = bounds.min[1];
                if (grid[w, l] == Tiles.Empty)
                {
                    //placeholder object, should use prefabs for final
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "EMPTY";
                    cube.transform.localScale = new Vector3(block_size, storey_height, block_size);
                    cube.transform.position = new Vector3(x + 0.5f, y + storey_height / 2.0f, z + 0.5f);
                    cube.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
                }
                else if (grid[w, l] == Tiles.Path)
                {
                    //placeholder object, should use prefabs for final
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "PATH";
                    cube.transform.localScale = new Vector3(block_size, storey_height, block_size);
                    cube.transform.position = new Vector3(x + 0.5f, y + storey_height / 2.0f, z + 0.5f);
                    cube.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
                }
                else if (grid[w, l] == Tiles.Unplacable)
                {
                    //placeholder object, should use prefabs for final
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "UNPLACABLE";
                    cube.transform.localScale = new Vector3(block_size, storey_height, block_size);
                    cube.transform.position = new Vector3(x + 0.5f, y + storey_height / 2.0f, z + 0.5f);
                    cube.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f);
                }
            }
        }       
    }

    // Update is called once per frame
    void Update()
    {
        //should this be doing anything?
    }

    public Vector3 closestValidBlock(float x, float z) {
        int w = Math.Clamp((int)((x - bounds_min_x) / block_size), 0, width - 1);
        int l = Math.Clamp((int)((z - bounds_min_z) / block_size), 0, length - 1);
        if (grid[w, l] != Tiles.Empty)
        {
            return new Vector3(0, -2, 0);
        }
        return new Vector3(w*block_size + bounds_min_x, 0, l * block_size + bounds_min_z);
    }

    public void AddTower(float x, float z)
    {
        int w = (int)((x - bounds_min_x) / block_size);
        int l = (int)((z - bounds_min_z) / block_size);
        grid[w, l] = Tiles.Tower;

        RecalculatePaths();
    }

    public void RecalculatePaths()
    {
        // Inform all active enemies to recalculate their paths
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            // Convert enemy's current position to grid coordinates
            Vector2Int start = new Vector2Int(
                Mathf.RoundToInt(enemy.transform.position.x / block_size),
                Mathf.RoundToInt(enemy.transform.position.z / block_size)
            );

            // Convert endPoint's position to grid coordinates
            Vector2Int end = new Vector2Int(
                Mathf.RoundToInt(endPoint.position.x / block_size),
                Mathf.RoundToInt(endPoint.position.z / block_size)
            );

            // Calculate the new path
            List<Vector3> newPath = pathfinding.FindPath(start, end, block_size);
            if (newPath.Count > 0)
            {
                enemy.SetPath(newPath);
                Debug.Log($"Recalculated path for enemy at {enemy.transform.position}. New path length: {newPath.Count}");
            }
            else
            {
                Debug.LogWarning($"No path found for enemy at {enemy.transform.position} from {start} to {end}");
            }
        }
    }


}

public class PriorityQueue<T>
{
    SortedSet<PriorityQueueObject<T>> queue;
    int a = 0;

    public PriorityQueue() { queue = new SortedSet<PriorityQueueObject<T>>(); }

    public void Add(T obj, int priority)
    {
        queue.Add(new PriorityQueueObject<T>(obj, priority, a++));
    }

    public T Peak()
    {
        var o = queue.ElementAt(0);
        return o.obj;
    }
    public T Pop()
    {
        var o = queue.ElementAt(0);
        queue.Remove(o);
        return o.obj;
    }

    public int Count()
    {
        return queue.Count;
    }
    public bool Contains(T obj)
    {
        foreach (var item in queue)
        {
            if (EqualityComparer<T>.Default.Equals(item.obj, obj))
            {
                return true;
            }
        }
        return false;
    }

}


public class PriorityQueueObject<T> : IComparable<PriorityQueueObject<T>>
{
    public T obj { get; set; }
    public int priority { get; set; }
    public int ex { get; set; }

    public PriorityQueueObject(T obj, int priority, int ex)
    {
        this.obj = obj;
        this.priority = priority;
        this.ex = ex;
    }

    public int CompareTo(PriorityQueueObject<T> other)
    {
        int order = priority.CompareTo(other.priority);
        if (order == 0) return ex.CompareTo(other.ex);
        return order;

    }
}

