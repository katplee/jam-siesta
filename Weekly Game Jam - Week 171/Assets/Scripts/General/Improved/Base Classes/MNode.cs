using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class MNode : MonoBehaviour
{
    //the node's position is in cell coordinates
    public abstract Tilemap Tilemap { get; set; }
    public Vector3Int Position { get; set; } = new Vector3Int();
    public GameObject occupant = null;

    protected void SetPositionInTilemap()
    {
        Vector3Int position = Tilemap.WorldToCell(transform.position);
        Position = position;
    }

    public Vector3Int GetPositionInTileMap()
    {
        Vector3Int position = Tilemap.WorldToCell(transform.position);
        return position;
    }

    public MNode ParentObject(GameObject child)
    {
        //set the original parent's occupant to null
        bool parentExists = child.transform.parent.TryGetComponent(out MNode oldParent);
        if (parentExists)
        {
            oldParent.UnparentObject();
        }

        //set the new parent's occupant to child, and re-parent the child
        child.transform.SetParent(transform);
        occupant = child;

        return this;
    }

    public void UnparentObject()
    {
        occupant = null;
    }
}
