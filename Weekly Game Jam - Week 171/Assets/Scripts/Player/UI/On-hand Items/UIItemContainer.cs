using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIItemContainer : UIObject
{
    [SerializeField] private Sprite luggage;
    [SerializeField] private Sprite sheets;
    [SerializeField] private Sprite pajamas;
    [SerializeField] private Sprite empty;

    [SerializeField] private List<UIItem> items = new List<UIItem> { new UIItem(), new UIItem() };

    private void Awake()
    {
        Player.Instance.DeclareThis(Label, this);
    }

    private void Start()
    {
        UpdateItemContainer(null);
    }

    private void SetItem(UIItem item, bool isFirst)
    {
        int i = (isFirst) ? 0 : 1;
        items[i] = item;
    }

    public void DeclareThis<T>(string element, T UIObject, bool isFirst)
        where T : UIObject
    {
        switch (element)
        {
            case "UIItem":
                SetItem(UIObject as UIItem, isFirst);
                break;
        }
    }

    public void UpdateItemContainer(List<ItemTransferrable> transferrables)
    {
        if (transferrables == null) { RefreshList(); return; }

        List<ItemTransferrable> sorted = transferrables.OrderBy(x => x.label).ToList();

        if(sorted.Count == 0) { RefreshList(); return; }

        for (int i = 0; i < sorted.Count; i++)
        {
            items[i].ChangeSprite(SelectSprite(sorted[i].label));
        }
    }

    private Sprite SelectSprite(string name)
    {
        switch (name)
        {
            case "Luggage":
                return luggage;

            case "Sheets":
                return sheets;

            case "Pajamas":
                return pajamas;

            default:
                break;
        }

        return empty;
    }

    private void RefreshList()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].ChangeSprite(empty);
        }
    }
}
