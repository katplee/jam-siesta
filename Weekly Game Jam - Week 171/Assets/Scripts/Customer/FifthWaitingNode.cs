using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FifthWaitingNode : CustomerNode
{
    private CustomerSpawner spawner = null;
    private MNode positionNode = null;

    private new void Awake()
    {
        //get the customer spawner
        spawner = GetComponentInParent<CustomerSpawner>();

        //initialize as a customer node
        base.Awake();
    }

    public override MNode ParentObject(GameObject child)
    {
        TileBase floor = Tilemap.GetTile(new Vector3Int(0, 11, 0));

        //set the new parent's occupant to child, and re-parent the child
        child.transform.SetParent(transform);
        occupant = child;

        //set the tile at the new parent node to be null
        Tilemap.SetTile(GetPositionInTileMap(), null);

        return this;
    }

    protected override void Dequeue()
    {
        if (spawner.HasCustomersToSpawn())
        {
            Debug.Log("dequeue");
            //dequeue the queue in the spawner
            CustomerController controller = spawner.Dequeue();
            if (!controller) { return; }
            
            positionNode = ParentObject(controller.gameObject);
            controller.InvokeSpecialMoveCompleteEvent(positionNode);
        }
    }
}
