using System.Collections.Generic;
using UnityEngine;


public class Node : IHeapItem<Node>
{
    public bool IsWalkable;
    public Vector3 WorldPosition;
    public int GridX, GridY;
    public int gCost, hCost;
    public Node Parent;
    public int HeapIndex { get; set; } 

    public int FCost => gCost + hCost;

    public int CompareTo(Node other)
    {
        int compare = FCost.CompareTo(other.FCost);
        if (compare == 0) compare = hCost.CompareTo(other.hCost); 
        return -compare; 
    }
}

public class PathFindingGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2 gridWorldSize = new Vector2(10f, 10f); 
    public float nodeRadius = 0.5f; 
    public LayerMask obstacleMask; 

    [Header("Debug")]
    public bool showGridGizmos = true;
    public Color walkableColor = Color.green;
    public Color blockedColor = Color.red;

    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * (gridWorldSize.x / 2) - Vector3.up * (gridWorldSize.y / 2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //world position 
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
               
                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, obstacleMask);

                grid[x, y] = new Node
                {
                    IsWalkable = walkable,
                    WorldPosition = worldPoint,
                    GridX = x,
                    GridY = y,
                    gCost = int.MaxValue,  
                    hCost = 0,
                    Parent = null,
                    HeapIndex = 0          
                };
            }
        }
    }

    // Convert world position to grid coordinates
    public Node GetNodeFromWorldPos(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    // Debug visualization
    void OnDrawGizmos()
    {
        if (showGridGizmos && grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = node.IsWalkable ? walkableColor : blockedColor;
                Gizmos.DrawWireCube(node.WorldPosition, Vector3.one * (nodeDiameter - 0.03f));
            }
        }
    }

    public Node GetNodeBelow(Node node)
    {
        if (node.GridY == 0) return null;
        return grid[node.GridX, node.GridY - 1];
    }

    public Node GetNodeInDirection(Node node, int dirX, int dirY)
    {
        int newX = node.GridX + dirX;
        int newY = node.GridY + dirY;

        if (newX < 0 || newX >= gridSizeX || newY < 0 || newY >= gridSizeY)
            return null;

        return grid[newX, newY];
    }
    public List<Node> GetNeighbors(Node node, bool allowDiagonal = true)
    {
        List<Node> neighbors = new List<Node>();

        // Check orthogonal directions (up, down, left, right)
        CheckDirection(node, 0, 1, neighbors); // Up
        CheckDirection(node, 1, 0, neighbors); // Right
        CheckDirection(node, 0, -1, neighbors); // Down
        CheckDirection(node, -1, 0, neighbors); // Left

        // Optional: Check diagonals (if allowed)
        if (allowDiagonal)
        {
            CheckDirection(node, 1, 1, neighbors); // Up-Right
            CheckDirection(node, 1, -1, neighbors); // Down-Right
            CheckDirection(node, -1, 1, neighbors); // Up-Left
            CheckDirection(node, -1, -1, neighbors); // Down-Left
        }

        return neighbors;
    }

    private void CheckDirection(Node node, int dirX, int dirY, List<Node> neighbors)
    {
        int checkX = node.GridX + dirX;
        int checkY = node.GridY + dirY;

        // Check if neighbor is within grid bounds
        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
        {
            Node neighbor = grid[checkX, checkY];

            // Optional: Add custom neighbor validation here
            // (e.g., check for dynamic obstacles)
            neighbors.Add(neighbor);
        }
    }
    public int MaxSize => gridSizeX * gridSizeY;
}
