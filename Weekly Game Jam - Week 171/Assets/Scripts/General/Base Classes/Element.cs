using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Element : MonoBehaviour, IUserInterface
{
    protected GameObject location = null;
    public Vector3Int Position { get; set; }
    public abstract Tilemap Tilemap { get; set; }

    public string label
    {
        get { return GetType().Name; }
    }

    protected List<ItemTransferrable> itemsInHand = new List<ItemTransferrable>();

    public Vector3Int GetPositionInTilemap()
    {
        Vector3Int position = Tilemap.WorldToCell(transform.position);
        return position;
    }

    public bool GiveItemTo(Element receiver, ItemTransferrable itemType)
    {
        if (!ReleaseItem(itemType, out List<ItemTransferrable> items)) { return false; }

        ItemTransferrable[] itemsArray = items.ToArray();

        bool received = receiver.ReceiveItem(itemsArray);

        return received;
    }

    public bool ReceiveItem(ItemTransferrable[] items)
    {
        //problem down the line: what if at the moment, only 1 item can be received by the player?
        //can the player go back to it again at another time?

        bool received = false;

        foreach (ItemTransferrable item in items)
        {
            if (this as Player && itemsInHand.Count == 2) { received = received || false; }
            else if (this as Customer && itemsInHand.Count == 1) { received = received || false; }
            
            else { itemsInHand.Add(item); received = received || true; }
            item.transform.SetParent(transform);
            item.SetOwner(); //sets the parent game element as the parent
        }

        return received;
    }

    protected bool ReleaseItem<T>(T itemType, out List<ItemTransferrable> items)
        where T : ItemTransferrable
    {
        List<ItemTransferrable> itemsOfType = new List<ItemTransferrable>();
        List<ItemTransferrable> _itemsInHand = new List<ItemTransferrable>(itemsInHand);

        foreach (ItemTransferrable i in itemsInHand)
        {
            if (i as T) { itemsOfType.Add(i); _itemsInHand.Remove(i); }
        }

        itemsInHand = _itemsInHand;

        bool released = (itemsOfType.Count != 0) ? true : false;
        items = itemsOfType;
        return released;
    }
}
