using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class ItemClickable : Clickable, IUserInterface
{
    protected Transform playerNode = null;
    protected Transform customerNode = null;
    protected Transform itemNode = null;
    private Tilemap playerTilemap = null;

    public Transform container { get; private set; } = null;
    protected ItemTransferrable content = null;
    protected GameObject contentObject;
    private List<ItemTransferrable> itemsInStock = new List<ItemTransferrable>();
    [SerializeField] private DestinationScriptable destination;


    public string label
    {
        get { return name; }
    }

    protected virtual void Awake()
    {
        SubscribeEvent();

        playerNode = GetComponentInChildren<PlayerNode>().transform;
        customerNode = (GetComponentInChildren<CustomerNode>()) ?
            GetComponentInChildren<CustomerNode>().transform : null;
        itemNode = (GetComponentInChildren<ItemNode>()) ?
            GetComponentInChildren<ItemNode>().transform : null;
        container = (GetComponentInChildren<ItemContainer>()) ?
            GetComponentInChildren<ItemContainer>().transform : transform;
        playerTilemap = TilemapManager.Instance.playerTilemap;
    }

    private void OnDestroy()
    {
        UnsubscribeEvent();
    }

    public Vector3Int GetPositionInTileMap()
    {
        //returns the position of the corresponding node, not of the item
        Vector3Int position = playerTilemap.WorldToCell(playerNode.position);
        return position;
    }

    public override void OnClick()
    {
        //the player is brought to the node corresponding to the item clicked
        //note: position conversion to cell position will be done inside the move player method
        PlayerController.Instance.TransportPlayer(playerNode);
        Player.Instance.RestartTags();

        //update the task panel by adding the icon on this clickable item

    }

    protected ItemTransferrable GenerateContent()
    {
        contentObject = new GameObject();
        contentObject.transform.SetParent(container);
        contentObject.name = content.GetType().Name;
        ItemTransferrable component = contentObject.AddComponent(content.GetType()) as ItemTransferrable;
        return component;
    }

    protected virtual void OnInteractionWithItem(MNode playerPosition)
    {
        if (GetPositionInTileMap() == playerPosition.GetPositionInTileMap())
        {
            Interact();
        }
    }

    protected virtual void Interact() { }

    public bool ReceiveItem(List<ItemTransferrable> items)
    {
        foreach (ItemTransferrable item in items)
        {
            itemsInStock.Add(item);
            item.transform.SetParent(container);
        }

        return true;
    }

    public bool ReleaseItem<T>(int quantity, T itemType, Element owner, out List<ItemTransferrable> items)
        where T : ItemTransferrable
    {
        List<ItemTransferrable> itemsOfType = new List<ItemTransferrable>();
        List<ItemTransferrable> _itemsInStock = new List<ItemTransferrable>(itemsInStock);

        foreach (ItemTransferrable i in itemsInStock)
        {
            if (quantity == 0) { break; }

            if (owner) { if (i.GetOwner() != owner) { continue; } }

            if (i as T) { itemsOfType.Add(i); _itemsInStock.Remove(i); quantity--; }
        }

        itemsInStock = _itemsInStock;

        bool released = (itemsOfType.Count != 0) ? true : false;
        items = itemsOfType;
        return released;
    }

    public ItemNode GetItemNode()
    {
        if (!itemNode) { return null; }
        return itemNode.GetComponent<ItemNode>();
    }

    public CustomerNode GetCustomerNode()
    {
        if (!customerNode) { return null; }
        return customerNode.GetComponent<CustomerNode>();
    }

    public PlayerNode GetPlayerNode()
    {
        if (!playerNode) { return null; }
        return playerNode.GetComponent<PlayerNode>();
    }

    private void SubscribeEvent()
    {
        PlayerController.OnMoveComplete += OnInteractionWithItem;
    }

    private void UnsubscribeEvent()
    {
        PlayerController.OnMoveComplete -= OnInteractionWithItem;

    }
}
