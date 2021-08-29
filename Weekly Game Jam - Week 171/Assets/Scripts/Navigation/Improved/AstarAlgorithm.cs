using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AstarAlgorithm
{
    //parameters to be used in the constructor
    private Vector3Int startPosition;
    private Vector3Int endPosition;
    private Tilemap tilemap; //needs to be assigned
    
    //
    private Node current = null;
    private HashSet<Node> openList;
    private HashSet<Node> closedList;
    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    private Stack<Vector3Int> finalPath = null;

    public AstarAlgorithm(Vector3Int startPosition, Vector3Int endPosition, Tilemap tilemap)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.tilemap = tilemap;
    }

    public Stack<Vector3Int> FindPath()
    {
        ImplementAstar();
        return finalPath;
    }

    private void ImplementAstar()
    {
        if (finalPath == null)
        {
            if (current == null)
            {
                Initialize();
            }

            while (openList.Count > 0)
            {
                List<Node> neighbors = FindNeighbors(current.Position);
                ExamineNeighbors(neighbors, current);
                UpdateCurrentNode(ref current);
                bool foundEndPosition = GenerateFinalPath(current);

                if (foundEndPosition) { return; }
            }
        }
    }

    private void Initialize()
    {
        openList = new HashSet<Node>();
        closedList = new HashSet<Node>();
        current = GetNode(startPosition);
        openList.Add(current);
    }

    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))
        {
            return allNodes[position];
        }
        else
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }

    private List<Node> FindNeighbors(Vector3Int parentPos)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (y != 0 && x != 0) { continue; }

                Vector3Int neighborPos = new Vector3Int(parentPos.x + x, parentPos.y + y, parentPos.z);

                if (neighborPos != startPosition && tilemap.GetTile(neighborPos))
                {
                    Node node = GetNode(neighborPos);
                    neighbors.Add(node);
                }
            }
        }
        return neighbors;
    }

    private void ExamineNeighbors(List<Node> neighbors, Node current)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            Node neighbor = neighbors[i];

            int gScore = CalculateGScore(current.Position, neighbor.Position);

            if (openList.Contains(neighbor))
            {
                if (current.G + gScore < neighbor.G)
                {
                    CalculateNeighborValues(current, neighbor, gScore);
                }
            }
            //if not in the closed list, but is also not yet in the open list
            else if (!closedList.Contains(neighbor))
            {
                CalculateNeighborValues(current, neighbor, gScore);
                openList.Add(neighbor);
            }
        }
    }

    private int CalculateGScore(Vector3Int currentPos, Vector3Int neighborPos)
    {
        Vector3Int distance = currentPos - neighborPos;

        if ((Mathf.Abs(distance.x) + Mathf.Abs(distance.y)) % 2 == 1) { return 10; }
        else { return 14; }
    }

    private void CalculateNeighborValues(Node parent, Node neighbor, int cost)
    {
        neighbor.Parent = parent;

        neighbor.G = parent.G + cost;
        neighbor.H = (Mathf.Abs(endPosition.x - neighbor.Position.x) + Mathf.Abs(endPosition.y - neighbor.Position.y)) * 10;
        neighbor.F = neighbor.G + neighbor.H;
    }

    private void UpdateCurrentNode(ref Node current)
    {
        openList.Remove(current);
        closedList.Add(current);

        if (openList.Count > 0)
        {
            current = openList.OrderBy(node => node.F).First();
        }
    }

    private bool GenerateFinalPath(Node current)
    {
        if (current.Position == endPosition)
        {
            Stack<Vector3Int> path = new Stack<Vector3Int>();

            while (current.Position != startPosition)
            {
                path.Push(current.Position);
                current = current.Parent;
            }

            finalPath = path;

            return true;
        }

        return false;
    }

}
