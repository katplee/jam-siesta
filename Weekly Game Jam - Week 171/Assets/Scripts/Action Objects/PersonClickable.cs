using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PersonClickable : Clickable
{
    private MNode node = null;
    private Element element = null;

    private void Awake()
    {
        node = GetComponentInChildren<MNode>();
        element = GetComponent<Element>();
    }

    private void Start()
    {
        //UpdateChildNode();
    }

    public override void OnClick()
    {
        //the player is brought to the node corresponding to the item clicked
        //note: position conversion to cell position will be done inside the move player method
        PlayerController.Instance.TransportPlayer(node.transform);
    }

    public void UpdateChildNode()
    {
        MNode _node = GameManager.Instance.SearchClosestNode(node.GetPositionInTileMap(), element.label);
        node.MoveToPositionInTilemap(_node.transform.position);
    }
    
    public Vector3Int GetPositionInTileMap()
    {
        //returns the position of the corresponding node, not of the person
        return node.GetPositionInTileMap();
    }
    
}
