using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Level: MonoBehaviour
{
    enum Tiles
    {
        Unplacable = -1,
        Empty =0,
        Path = 1,
        Tower =2,
    }

    

    int bounds_min_x;
    int bounds_min_z;
    int bounds_max_x;
    int bounds_max_z;
    int bounds_size_x;
    int bounds_size_z;
    int block_size;

    int width;
    int length;
    int storey_height = 2;
    Tiles[,] grid;
    private Bounds bounds;

    // Start is called before the first frame update
    void Start()
    {
        // move to globals or some sort of file storage
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

        //randgrid is for testing ONLY
        randGrid();
        drawgrid();
    }

    void randGrid()
    {
        if (grid == null)
        {
            grid = new Tiles[width, length];
        }
        Tiles[] gen = new Tiles[] { Tiles.Unplacable, Tiles.Empty, Tiles.Path };
        for (int i = 0; i < width; i++)
            for (int j = 0; j < length; j++)
                grid[i, j] = gen[UnityEngine.Random.Range(0, 3)];
    }

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
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "EMPTY";
                    cube.transform.localScale = new Vector3(block_size, storey_height, block_size);
                    cube.transform.position = new Vector3(x + 0.5f, y + storey_height / 2.0f, z + 0.5f);
                    cube.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
                }
                else if (grid[w, l] == Tiles.Path)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "PATH";
                    cube.transform.localScale = new Vector3(block_size, storey_height, block_size);
                    cube.transform.position = new Vector3(x + 0.5f, y + storey_height / 2.0f, z + 0.5f);
                    cube.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
                }
                else if (grid[w, l] == Tiles.Unplacable)
                {
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

