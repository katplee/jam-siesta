using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoney : UIObject
{
    private UIMoneyContainer parent;
    private Text text;
    private Image image;

    private void Awake()
    {
        parent = GetComponentInParent<UIMoneyContainer>();
        text = GetComponent<Text>();
        image = GetComponent<Image>();

        if (parent)
        {
            parent.DeclareThis(Label, this, transform.GetSiblingIndex());
        }
    }

    public void ChangeText(char newText)
    {
        text.text = newText.ToString();
    }

    public void ChangeSprite(Sprite newSprite)
    {
        image.sprite = newSprite;
    }
}
