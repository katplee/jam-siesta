using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Element : MonoBehaviour
{
    protected GameObject location = null;
    public Vector3Int Position { get; set; }
    public abstract Tilemap Tilemap { get; set; }
    protected List<ItemTransferrable> itemsInHand = new List<ItemTransferrable>();

    public Vector3Int GetPositionInTilemap()
    {
        Vector3Int position = Tilemap.WorldToCell(transform.position);
        return position;
    }

    public bool GiveItemTo(Element receiver, ItemTransferrable itemType)
    {
        if (!ReleaseItem(itemType, out ItemTransferrable item)) { return false; }

        bool received = receiver.ReceiveItem(item);

        return received;
    }

    public bool ReceiveItem(ItemTransferrable item)
    {
        //item was not received
        if (this as Player && itemsInHand.Count == 2) { return false; }
        else if (this as Customer && itemsInHand.Count == 1) { return false; }
        else { itemsInHand.Add(item); }

        item.transform.SetParent(transform);

        return true;
    }

    private bool ReleaseItem<T>(T itemType, out ItemTransferrable item)
        where T : ItemTransferrable
    {
        foreach (ItemTransferrable i in itemsInHand)
        {
            if(i as T)
            {
                item = i;
                return itemsInHand.Remove(i);
            }
        }

        item = null;
        return false;
    }
}
