using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AstarDebugger : MonoBehaviour
{
    private static AstarDebugger instance;

    public static AstarDebugger MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AstarDebugger>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private Tilemap tileMap;

    [SerializeField]
    private Tile tile;

    [SerializeField]
    private Color openColor, closedColor, pathColor, currentColor, startColor, goalColor;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject debugTextPrefab;

    private List<GameObject> debugObjects = new List<GameObject>();

    public void CreateTiles
        (HashSet<Node> openList, HashSet<Node> closedList, Dictionary<Vector3Int, Node> allNodes, Vector3Int start, Vector3Int goal, Stack<Vector3Int> path = null)
    {
        foreach(Node node in openList)
        {
            ColorTile(node.Position, openColor);
        }

        foreach(Node node in closedList)
        {
            ColorTile(node.Position, closedColor);
        }        

        if(path != null)
        {
            foreach (Vector3Int pos in path)
            {
                if (pos != start || pos != goal)
                {
                    ColorTile(pos, pathColor);
                }
            }
        }

        ColorTile(start, startColor);
        ColorTile(goal, goalColor);

        foreach (KeyValuePair<Vector3Int, Node> node in allNodes)
        {
            if(node.Value.Parent != null)
            {
                GameObject go = Instantiate(debugTextPrefab, canvas.transform);
                go.transform.position = grid.CellToWorld(node.Key);
                GenerateDebugText(node.Value, go.GetComponent<DebugText>());
                debugObjects.Add(go);
            }
        }
    }

    private void GenerateDebugText(Node node, DebugText debugText)
    {
        int x = node.Position.x - node.Parent.Position.x;
        int y = node.Position.y - node.Parent.Position.y;

        float angle = Mathf.Atan2(y, x);
        angle = angle * Mathf.Rad2Deg + 180;

        debugText.MyArrow.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        debugText.P.text = $"P: {node.Position.x},{node.Position.y}";
        debugText.F.text = $"F: {node.F}";
        debugText.G.text = $"G: {node.G}";
        debugText.H.text = $"H: {node.H}";
    }

    public void ColorTile(Vector3Int position, Color color)
    {
        tileMap.SetTile(position, tile);
        tileMap.SetTileFlags(position, TileFlags.None); //why is this necessary??
        tileMap.SetColor(position, color);    
    }

}
