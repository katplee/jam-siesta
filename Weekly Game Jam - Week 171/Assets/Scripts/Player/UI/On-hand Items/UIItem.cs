using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : UIObject
{
    private UIItemContainer parent;
    private Text text;
    private Image image;
    private int index;

    private void Awake()
    {
        parent = GetComponentInParent<UIItemContainer>();
        text = GetComponent<Text>();
        index = transform.parent.GetSiblingIndex() - 1;

        if (parent)
        {
            bool index_bool = (index == 0) ? true : false;
            parent.DeclareThis(Label, this, index_bool);
        }
    }

    public void ChangeText(string newText)
    {
        text.text = newText;
    }

    public void ChangeSprite(Sprite newSprite)
    {
        image.sprite = newSprite;
    }
}
