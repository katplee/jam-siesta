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
        SetPositionInTilemap();

        //add this node to the dictionary of customer nodes
        GameManager.Instance.AddNode(GetType().Name, Position, this);
    }
}
