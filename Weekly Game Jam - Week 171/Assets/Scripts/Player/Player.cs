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

    //UI-related parameters
    private UIItemContainer itemContainerUI = null;

    public override Tilemap Tilemap { get; set; }

    private void Awake()
    {
        SubscribeEvent();

        performance = GetComponent<PlayerPerformance>();
        Tilemap = TilemapManager.Instance.playerTilemap;
    }
    private void OnDestroy()
    {
        UnsubscribeEvent();
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
    {
        bool released = base.ReleaseItem(quantity, itemType, mustBeClean, out items);

        if (released) { OnItemUpdate?.Invoke(); }

        return released;
    }

    private void UpdateItems()
    {
        itemContainerUI.UpdateItemContainer(itemsInHand);
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

    public void DeclareThis<T>(string element, T UIobject)
        where T : UIObject
    {
        switch (element) 
        {
            case "UIItemContainer":
                itemContainerUI = UIobject as UIItemContainer;
                break;
        }
    }

    private void SubscribeEvent()
    {
        OnItemUpdate += UpdateItems;
    }

    private void UnsubscribeEvent()
    {
        OnItemUpdate -= UpdateItems;
    }
}
