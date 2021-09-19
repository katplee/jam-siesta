using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class MNode : MonoBehaviour
{
    public abstract Tilemap Tilemap { get; set; }
    protected GameObject occupant = null;

    public void MoveToPositionInTilemap(Vector3 position)
    {
        transform.position = position;
    }

    public Vector3Int GetPositionInTileMap()
    {
        Vector3Int position = Tilemap.WorldToCell(transform.position);
        return position;
    }

    public virtual MNode ParentObject(GameObject child)
    {
        TileBase floor = Tilemap.GetTile(new Vector3Int(0, 11, 0));

        //set the original parent's occupant to null
        bool parentExists = child.transform.parent.TryGetComponent(out MNode oldParent);
        if (parentExists)
        {
            oldParent.UnparentObject();

            //set the old tile at the old parent node to be active
            Tilemap.SetTile(oldParent.GetPositionInTileMap(), floor);
        }

        //set the new parent's occupant to child, and re-parent the child
        child.transform.SetParent(transform);
        occupant = child;

        //set the tile at the new parent node to be null
        Tilemap.SetTile(GetPositionInTileMap(), null);


        return this;
    }

    public bool IsOccupied()
    {
        bool occupied = (occupant != null) ? true : false;
        return occupied;
    }

    public void UnparentObject()
    {
        occupant = null;
    }

    public GameObject GetOccupant()
    {
        if (occupant) { return occupant; }
        return null;
    }
}
