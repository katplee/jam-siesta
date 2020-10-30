using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType { START, GOAL }

public class AstarAlgo : MonoBehaviour
{
    private TileType tileType;

    [SerializeField]
    private Camera camera;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private Tile[] tiles;
    [SerializeField]
    private Vector3Int startPos, goalPos;

    private Node current;

    private HashSet<Node> openList;

    private HashSet<Node> closedList;

    private Stack<Vector3Int> path;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

            if(hit.collider != null)
            {
                Debug.Log("A collider was hit!");
                Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int tilePos = tileMap.WorldToCell(mouseWorldPos);

                ChangeTile(tilePos);
            }

        }

        if (Input.GetKeyDown(KeyCode.Space)){
            Algorithm();
        }
    }

    private void Algorithm()
    {
        if(current == null)
        {
            Initialize();
        }

        while(openList.Count > 0 && path == null)
        {
            List<Node> neighbors = FindNeighbors(current.Position);

            ExamineNeighbors(neighbors, current);

            UpdateCurrentNode(ref current);

            path = GeneratePath(current);
        }        

        AstarDebugger.MyInstance.CreateTiles(openList, closedList, allNodes, startPos, goalPos, path);
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
                Vector3Int neighborPos = new Vector3Int (parentPos.x + x, parentPos.y + y, parentPos.z);

                if (neighborPos != startPos && tileMap.GetTile(neighborPos))
                {
                    if (y != 0 || x != 0)
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
                if(current.G + gScore < neighbor.G)
                {
                    CalculateValues(current, neighbor, gScore);
                }
            }
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
        neighbor.H = (Mathf.Abs(goalPos.x - neighbor.Position.x) + Mathf.Abs(goalPos.y - neighbor.Position.y)) * 10;
        neighbor.F = neighbor.G + neighbor.H;

    }

    private int CalculateGScore(Vector3Int currentPos, Vector3Int neighborPos)
    {
        Vector3Int distance = currentPos - neighborPos;

        if(Mathf.Abs(distance.x) + Mathf.Abs(distance.y) % 2 == 0) { return 10; }
        else { return 14; }
    }

    private void UpdateCurrentNode(ref Node current)
    {
        openList.Remove(current);
        closedList.Add(current);

        if(openList.Count > 0)
        {
            current = openList.OrderBy(node => node.F).First();
        }
    }       

    public void ChangeTileType(TileButton button)
    {
        tileType = button.MyTileType;
    }

    private void ChangeTile(Vector3Int clickPos)
    {   
        tileMap.SetTile(clickPos, tiles[(int)tileType]);

        if (tileType == TileType.START) { startPos = clickPos; }
        else if (tileType == TileType.GOAL) { goalPos = clickPos; }
    }

    private Stack<Vector3Int> GeneratePath(Node current)
    {
        if(current.Position == goalPos)
        {
            Stack<Vector3Int> path = new Stack<Vector3Int>();

            while(current.Position != startPos)
            {
                path.Push(current.Position);

                current = current.Parent;
            }
            return path;
        }
        return null;        
    }
}
