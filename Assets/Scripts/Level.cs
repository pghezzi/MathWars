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

    int bounds_min_x;
    int bounds_min_z;
    int bounds_max_x;
    int bounds_max_z;
    int bounds_size_x;
    int bounds_size_z;
    int block_size;


    int width;
    int length;
    int storey_height   = 2;
    private Bounds bounds;
    public Tiles[,] grid;
    public string level_name; 

    //  Start is called before the first frame update
    void Start()
    {
        //  move to globals or some sort of file storage
        bounds_min_x    = 0;
        bounds_min_z    = 0;
        block_size      = 10;

        //  file loc setup
        string project_directoy = Directory.GetCurrentDirectory() + "/Assets/Levels/";
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

