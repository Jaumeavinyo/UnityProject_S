using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinder : MonoBehaviour
{
    [Header("Pathfinding Conditions")]
    public int maxNonWalkableHeight = 2;       // Max air nodes before falling
    public int maxHoleLength = 3;              // Max non-walkable nodes horizontally
    public bool allowLedges = false;           // Forbid paths that drop off cliffs
    public float maxSlopeAngle = 45f;          // Maximum allowed ground slope
    public bool preferHorizontalMovement = true;// Bias toward horizontal paths
    public int nonWalkablePenalty = 10;        // Extra cost for risky paths
    public bool avoidMovingObstacles = true;   // Re-route around dynamic blockers

    private PathFindingGrid grid;
    private LayerMask obstacleMask;

    void Awake()
    {
        grid = GetComponent<PathFindingGrid>();
        obstacleMask = grid.obstacleMask;
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.GetNodeFromWorldPos(startPos);
        Node targetNode = grid.GetNodeFromWorldPos(targetPos);

        Heap<Node> UncheckedNodes = new Heap<Node>(grid.MaxSize);
        HashSet<Node> ChackedNodes = new HashSet<Node>();
        UncheckedNodes.Add(startNode);

        while (UncheckedNodes.Count > 0)
        {
            Node currentNode = UncheckedNodes.RemoveFirst();
            ChackedNodes.Add(currentNode);

            if (currentNode == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (ChackedNodes.Contains(neighbor)) continue;             
                if (!IsTraversable(neighbor)) continue;//obvious
                if (!IsValidHeight(currentNode, neighbor)) continue;//can jump this heigh?
                if (!IsValidHoleLength(currentNode, neighbor)) continue;//can jump this distance between tiles? (hole in the way)
                if (!IsValidSlope(currentNode, neighbor)) continue;//obvious
                if (!IsValidLedge(currentNode, neighbor)) continue;// edge jump to lower tile no mater hight TODO (make high mater)

                int movementCost = currentNode.gCost + GetDistance(currentNode, neighbor) + GetPenalty(neighbor);
                if (movementCost < neighbor.gCost || !UncheckedNodes.Contains(neighbor))
                {
                    neighbor.gCost = movementCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.Parent = currentNode;

                    if (!UncheckedNodes.Contains(neighbor))
                        UncheckedNodes.Add(neighbor);
                }
            }
        }
        return null;
    }

    // --- Condition Check Methods ---
    private bool IsTraversable(Node node)
    {
        // Avoid moving obstacles if enabled
        if (avoidMovingObstacles && Physics2D.OverlapCircle(node.WorldPosition, grid.nodeRadius, obstacleMask))
        {
            return false;
        }
            
        return true;
    }

    private bool IsValidHeight(Node from, Node to)
    {
        if (to.IsWalkable)
        {
            return true;
        }
        int airNodes = 0;
        Node nodeBelow = grid.GetNodeBelow(to);
        while (nodeBelow != null && !nodeBelow.IsWalkable && airNodes <= maxNonWalkableHeight)
        {
            airNodes++;
            nodeBelow = grid.GetNodeBelow(nodeBelow);
        }
        return airNodes <= maxNonWalkableHeight;
    }

    private bool IsValidHoleLength(Node from, Node to)
    {
        if (to.IsWalkable) return true;

        int holeLength = 0;
        Node node = to;
        while (node != null && !node.IsWalkable && holeLength <= maxHoleLength)
        {
            holeLength++;
            node = grid.GetNodeInDirection(node, from.GridX - to.GridX, from.GridY - to.GridY);
        }
        return holeLength <= maxHoleLength;
    }

    private bool IsValidSlope(Node from, Node to)
    {
        if (!from.IsWalkable || !to.IsWalkable) return true;

        float slope = Vector3.Angle(Vector3.up, to.WorldPosition - from.WorldPosition);
        return slope <= maxSlopeAngle;
    }

    private bool IsValidLedge(Node from, Node to)
    {
        if (allowLedges) return true;
        if (!from.IsWalkable || to.IsWalkable) return true;

        // Check if moving to a non-walkable node below (ledge drop)
        return to.GridY >= from.GridY;
    }

    private int GetPenalty(Node node)
    {
        return node.IsWalkable ? 0 : nonWalkablePenalty;
    }

    private int GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.GridX - b.GridX);
        int distY = Mathf.Abs(a.GridY - b.GridY);

        // Apply directional bias
        if (preferHorizontalMovement)
            return distX + (int)(distY * 1.5f); // Penalize vertical movement
        else
            return distX + distY;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        return path;
    }
}