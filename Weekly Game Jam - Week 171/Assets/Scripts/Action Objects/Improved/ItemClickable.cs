using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public abstract class ItemClickable : MonoBehaviour, IUserInterface
{
    private Transform node = null;
    private Tilemap playerTilemap = null;
    private List<ItemTransferrable> itemsinStock = new List<ItemTransferrable>();

    public string label
    {
        get { return name; }
    }

    private void Awake()
    {
        SubscribeEvent();
        node = transform.GetChild(0);
        playerTilemap = TilemapManager.Instance.playerTilemap;
    }

    private void OnDestroy()
    {
        UnsubscribeEvent();
    }

    public Vector3Int GetPositionInTileMap()
    {
        Vector3Int position = playerTilemap.WorldToCell(node.position);
        return position;
    }

    public virtual void OnClick()
    {
        //the player is brought to the node corresponding to the item clicked
        //note: position conversion to cell position will be done inside the move player method
        PlayerController.Instance.TransportPlayer(node);
    }

    protected abstract void OnInteractionWithItem(MNode playerPosition);

    public bool ReceiveItem(List<ItemTransferrable> items)
    {
        foreach (ItemTransferrable item in items)
        {
            itemsinStock.Add(item);
            item.transform.SetParent(transform);
        }

        return true;
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
