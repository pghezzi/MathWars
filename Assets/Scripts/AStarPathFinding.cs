using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AStarPathfinding
{
    private Tiles[,] grid;
    private int width;
    private int height;
    private Vector2Int[] directions = 
    {
        new Vector2Int(0, 1),  // Up
        new Vector2Int(1, 0),  // Right
        new Vector2Int(0, -1), // Down
        new Vector2Int(-1, 0)  // Left
    };

    public AStarPathfinding(Tiles[,] grid)
    {
        this.grid = grid;
        this.width = grid.GetLength(0);
        this.height = grid.GetLength(1);
    }

    public List<Vector3> FindPath(Vector2Int start, Vector2Int end, float blockSize)
    {
        PriorityQueue<Vector2Int> openSet = new PriorityQueue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float>();

        foreach (var x in Enumerable.Range(0, width))
        {
            foreach (var y in Enumerable.Range(0, height))
            {
                gScore[new Vector2Int(x, y)] = float.MaxValue;
                fScore[new Vector2Int(x, y)] = float.MaxValue;
            }
        }

        gScore[start] = 0;
        fScore[start] = Heuristic(start, end);
        openSet.Add(start, fScore[start]);

        while (openSet.Count() > 0)
        {
            Vector2Int current = openSet.Pop();
            if (current == end)
            {
                return ReconstructPath(cameFrom, current, blockSize);
            }

            foreach (var dir in directions)
            {
                Vector2Int neighbor = current + dir;

                if (IsInBounds(neighbor) && (grid[neighbor.x, neighbor.y] == Tiles.Path))
                {
                    float movementCost = 1 + UnityEngine.Random.Range(0f, 0.5f);
                    float tentativeGScore = gScore[current] + movementCost;
                    if (tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, end);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor, fScore[neighbor]);
                        }
                    }
                }
            }
        }

        return new List<Vector3>(); // Return an empty path if no path is found
    }

    private bool IsInBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan distance
    }


    private List<Vector3> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current, float blockSize)
    {
        List<Vector3> path = new List<Vector3>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(new Vector3(current.x * blockSize, 3f, current.y * blockSize));
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }

}

