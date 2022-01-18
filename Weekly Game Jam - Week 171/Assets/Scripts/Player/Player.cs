using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : Element
{
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    public static event Action OnItemUpdate;

    public PlayerPerformance performance { get; private set; } = null;
    
    //customer-related parameters
    private Tag activeTag = null;
    private Customer selectedCustomer = null;

    public override Tilemap Tilemap { get; set; }

    private void Awake()
    {
        performance = GetComponent<PlayerPerformance>();
        Tilemap = TilemapManager.Instance.playerTilemap;
    }

    public Tag SelectCustomer(Customer customer, Tag customerTag)
    {
        activeTag = customerTag;
        selectedCustomer = customer;
        return activeTag;
    }

    public override bool ReceiveItem(ItemTransferrable[] items)
    {
        bool received = base.ReceiveItem(items);

        if (received) { OnItemUpdate?.Invoke(); }

        return received;
    }

    protected override bool ReleaseItem<T>(int quantity, T itemType, bool mustBeClean, out List<ItemTransferrable> items)
        where T : ItemTransferrable
    {

    }

    public void RestartTags()
    {
        activeTag = null;
        selectedCustomer = null;
    }

    public Tag GetActiveTag(out Customer customer)
    {
        if(activeTag != null) { customer = selectedCustomer; }
        else { customer = null; }
        return activeTag;
    }
}
