using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerNode : MNode
{
    public override Tilemap Tilemap { get; set; }

    private void Awake()
    {
        //set the position and child order field parameter
        Tilemap = TilemapManager.Instance.playerTilemap;

        //add this node to the dictionary of customer nodes
        //except if it is the designated player node of a customer
        if (GetComponentInParent<Customer>()) { return; }

        GameManager.Instance.AddNode(GetType().Name, GetPositionInTileMap(), this);
    }
}
