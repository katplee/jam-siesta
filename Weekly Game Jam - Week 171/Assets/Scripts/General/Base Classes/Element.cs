using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Element : MonoBehaviour, IUserInterface
{
    protected Transform location = null;
    public abstract Tilemap Tilemap { get; set; }

    public string label
    {
        get { return GetType().Name; }
    }

    protected List<ItemTransferrable> itemsInHand = new List<ItemTransferrable>();

    public Transform UpdateLocation()
    {
        MNode location = GameManager.Instance.SearchEquivalentNode(GetPositionInTilemap(), label);
        this.location = location.transform;
        return this.location;
    }

    public Vector3Int GetPositionInTilemap()
    {
        Vector3Int position = Tilemap.WorldToCell(transform.position);
        return position;
    }

    public bool DropItemTo<T>(ItemClickable storage, T itemType)
        where T : ItemTransferrable
    {
        if (!ReleaseItem(10, itemType, false, out List<ItemTransferrable> items)) { return false; }

        bool dropped = storage.ReceiveItem(items);

        return dropped;
    }

    public bool GetItemFrom<T>(ItemClickable storage, int quantity, T itemType, out List<ItemTransferrable> items, Element owner = null)
        where T : ItemTransferrable
    {
        quantity = ComputeQuantity(quantity);

        if (!storage.ReleaseItem(quantity, itemType, owner, out List<ItemTransferrable> _items)) { items = _items; return false; }

        ItemTransferrable[] itemsArray = _items.ToArray();

        bool received = ReceiveItem(itemsArray);

        items = _items;
        return received;
    }

    public bool GiveItemTo<T>(Element receiver, T itemType)
        where T : ItemTransferrable
    {
        int quantity = receiver.ComputeQuantity(-1);

        if (!ReleaseItem<T>(quantity, itemType, true, out List<ItemTransferrable> items)) { return false; }

        ItemTransferrable[] itemsArray = items.ToArray();

        bool received = receiver.ReceiveItem(itemsArray);

        return received;
    }

    public bool ReceiveItem(ItemTransferrable[] items)
    {
        //problem down the line: what if at the moment, only 1 item can be received by the player?
        //can the player go back to it again at another time?
        //add an additional step where the receiver will return the items to the giver!

        bool received = false;

        foreach (ItemTransferrable item in items)
        {
            if (this as Player && itemsInHand.Count == 2) { received = received || false; }
            else if (this as Customer && itemsInHand.Count == 1) { received = received || false; }

            else
            {
                itemsInHand.Add(item); received = received || true;
                item.transform.SetParent(transform);
                //item.SetOwner(); //sets the parent game element as the parent
            }
        }
        return received;
    }

    protected bool ReleaseItem<T>(int quantity, T itemType, bool mustBeClean, out List<ItemTransferrable> items)
        where T : ItemTransferrable
    {
        List<ItemTransferrable> itemsOfType = new List<ItemTransferrable>();
        List<ItemTransferrable> _itemsInHand = new List<ItemTransferrable>(itemsInHand);

        foreach (ItemTransferrable i in itemsInHand)
        {
            if (quantity == 0) { break; }

            if (mustBeClean && i.GetComponent<Dirty>()) { continue; }

            if (i as T) { itemsOfType.Add(i); _itemsInHand.Remove(i); quantity--; }
        }

        itemsInHand = _itemsInHand;

        bool released = (itemsOfType.Count != 0) ? true : false;
        items = itemsOfType;
        return released;
    }

    private int ComputeQuantity(int required)
    {
        int output;
        int canReceive = 0;
        if (this as Player) { canReceive = 2 - itemsInHand.Count; }
        else if (this as Customer) { canReceive = 1 - itemsInHand.Count; }

        if (required < 0) { output = canReceive; }
        else { output = Mathf.Min(required, canReceive); }

        return output;
    }
}
