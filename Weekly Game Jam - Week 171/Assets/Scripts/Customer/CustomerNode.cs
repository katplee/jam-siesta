using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomerNode : MNode
{
    public override Tilemap Tilemap { get; set; }
    private Queue<CustomerController> waitList = new Queue<CustomerController>();
    private int childOrder = -1;

    private void Awake()
    {
        //set the position and child order field parameter
        Tilemap = TilemapManager.Instance.customerTilemap;
        childOrder = transform.GetSiblingIndex();

        //add this node to the dictionary of customer nodes
        GameManager.Instance.AddNode(GetType().Name, GetPositionInTileMap(), this);
    }

    public override MNode ParentObject(GameObject child)
    {
        TileBase floor = Tilemap.GetTile(new Vector3Int(0, 11, 0));

        //set the original parent's occupant to null
        bool parentExists = child.transform.parent.TryGetComponent(out MNode oldParent);
        if (parentExists)
        {
            oldParent.UnparentObject();

            Debug.Log(oldParent.GetPositionInTileMap());
            //set the old tile at the old parent node to be active
            Tilemap.SetTile(oldParent.GetPositionInTileMap(), floor);

            (oldParent as CustomerNode).Dequeue();
        }

        //set the new parent's occupant to child, and re-parent the child
        child.transform.SetParent(transform);
        occupant = child;

        //set the tile at the new parent node to be null
        Tilemap.SetTile(GetPositionInTileMap(), null);

        return this;
    }

    public void MakeTileActive(Vector3Int tile)
    {
        TileBase floor = Tilemap.GetTile(new Vector3Int(0, 11, 0));
        Tilemap.SetTile(tile, floor);
    }

    public void Queue(CustomerController controller)
    {
        waitList.Enqueue(controller);
    }

    private void Dequeue()
    {
        if (waitList.Count != 0)
        {
            //calls the first customer in the waitlist to approach node
            CustomerController customer = waitList.Dequeue();
            customer.TransportCustomer(transform);
        }
    }
}
