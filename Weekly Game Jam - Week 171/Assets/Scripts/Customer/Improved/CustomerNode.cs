using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomerNode : MNode
{
    public override Tilemap Tilemap { get; set; }
    private int childOrder = -1;

    private void Awake()
    {
        SubscribeEvents();

        //set the position and child order field parameter
        Tilemap = TilemapManager.Instance.customerTileMap;
        SetPositionInTilemap();
        childOrder = transform.GetSiblingIndex();

        //add this node to the dictionary of customer nodes
        GameManager.Instance.AddNode(GetType().Name, Position, this);
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void MoveToFrontNode()
    {
        Transform frontNode = transform.root.GetChild(childOrder - 1);
        frontNode.GetComponent<CustomerNode>().ParentObject(occupant);
    }

    private void SubscribeEvents()
    {
        WaitingCustomerManager.OnCustomerUpdate += MoveToFrontNode;
    }

    private void UnsubscribeEvents()
    {
        WaitingCustomerManager.OnCustomerUpdate -= MoveToFrontNode;
    }
}
