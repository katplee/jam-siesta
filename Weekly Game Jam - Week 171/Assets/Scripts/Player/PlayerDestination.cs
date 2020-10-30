using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDestination : Singleton<PlayerDestination>
{
    public Vector3Int Destination { get; set; }        
    public PlayerController Controller { get; set; }
    private Node current;
    private Vector3Int startPos;
    private Tilemap playerTileMap;
    private HashSet<Node> openList;
    private HashSet<Node> closedList;
    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    private Stack<Vector3Int> finalPath;

    private void Awake()
    {
        Controller = gameObject.GetComponent<PlayerController>();
    }

    private void Start()
    {
        playerTileMap = Tilemaps.Instance.playerTileMap;
        current = null;
        finalPath = null;
        startPos = playerTileMap.WorldToCell(transform.position);
        Controller.Destination = Destination;
    }

    private void Update()
    {
        Vector3 trWorldPos = transform.position;
        Vector3Int trTilePos = playerTileMap.WorldToCell(trWorldPos);

        CheckIfInDestination(Destination, trTilePos);
    }

    public void CheckIfInDestination(Vector3Int destination, Vector3Int transformPosition)
    {
        if (Vector3Int.Distance(destination, transformPosition) < 0.1f)
        {
            Controller.InputVector = new Vector3(0f, 0f, 0f);
            Destroy(this);
        }
        else
        {
            SetDestinationByController(destination, transformPosition);
        }
    }

    public void SetDestinationByController(Vector3Int destination, Vector3Int transformPosition)
    {
        Vector3Int playerToIntDistance;
        float playerToIntDirection;
        Vector3 inputVector;

        playerToIntDistance = IntermediateDest(destination, transformPosition) - transformPosition;

        if (Mathf.Abs(playerToIntDistance.x) > Mathf.Abs(playerToIntDistance.y))
        {
            playerToIntDirection = Mathf.Sign(playerToIntDistance.x);
            inputVector = new Vector3(playerToIntDirection, 0, 0);
        }
        else
        {
            playerToIntDirection = Mathf.Sign(playerToIntDistance.y);
            inputVector = new Vector3(0, playerToIntDirection, 0);
        }
        Controller.InputVector = inputVector;
    }

    private Vector3Int IntermediateDest(Vector3Int destination, Vector3Int transformPosition)
    {
        AstarAlgorithm();

        if (finalPath != null && finalPath.Count > 0)
        {
            if (Vector3.Distance(transformPosition, finalPath.Peek()) < 0.05f)
            {
                finalPath.Pop();
            }
            return finalPath.Peek();            
        }
        else
        {
            return destination;
        }
    }

    private void AstarAlgorithm()
    {
        if(finalPath == null)
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

                GenerateFinalPath(current);                
            }
            //AstarDebugger.MyInstance.CreateTiles(openList, closedList, allNodes, startPos, Destination, finalPath);
        }
    }

    private void Initialize()
    {
        openList = new HashSet<Node>();
        closedList = new HashSet<Node>();
        current = GetNode(startPos);
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
                Vector3Int neighborPos = new Vector3Int(parentPos.x + x, parentPos.y + y, parentPos.z);

                if (neighborPos != startPos && playerTileMap.GetTile(neighborPos))
                {
                    if (y == 0 || x == 0)
                    {
                        Node node = GetNode(neighborPos);
                        neighbors.Add(node);
                    }
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
                    CalculateValues(current, neighbor, gScore);
                }
            }
            //if not in the closed list, but is also not yet in the open list
            else if (!closedList.Contains(neighbor))
            {
                CalculateValues(current, neighbor, gScore);
                openList.Add(neighbor);
            }
        }
    }

    private void CalculateValues(Node parent, Node neighbor, int cost)
    {
        neighbor.Parent = parent;

        neighbor.G = parent.G + cost;
        neighbor.H = (Mathf.Abs(Destination.x - neighbor.Position.x) + Mathf.Abs(Destination.y - neighbor.Position.y)) * 10;
        neighbor.F = neighbor.G + neighbor.H;
    }

    private int CalculateGScore(Vector3Int currentPos, Vector3Int neighborPos)
    {
        Vector3Int distance = currentPos - neighborPos;

        if (Mathf.Abs(distance.x) + Mathf.Abs(distance.y) % 2 == 0) { return 10; }
        else { return 14; }
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

    private void GenerateFinalPath(Node current)
    {
        if (current.Position == Destination)
        {
            Stack<Vector3Int> path = new Stack<Vector3Int>();

            while (current.Position != startPos)
            {
                path.Push(current.Position);
                current = current.Parent;
            }
            finalPath = path;
        }
        return;
    }    
}
