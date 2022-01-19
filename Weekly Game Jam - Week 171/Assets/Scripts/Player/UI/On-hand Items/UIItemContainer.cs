using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIItemContainer : UIObject
{
    private Dictionary<ItemTransferrable, Sprite> itemDictionary = new Dictionary<ItemTransferrable, Sprite>();
    private List<UIItem> items = new List<UIItem> { new UIItem(), new UIItem() };

    private void Awake()
    {
        Player.Instance.DeclareThis(Label, this);

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
            items[i].ChangeText(sorted[i].label);
        }
    }

    private void RefreshList()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].ChangeText("-");
        }
    }
}
