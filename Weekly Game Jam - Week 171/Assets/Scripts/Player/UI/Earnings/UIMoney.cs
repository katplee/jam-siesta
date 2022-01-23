using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoney : UIObject
{
    private UIMoneyContainer parent;
    private Text text;

    private void Awake()
    {
        parent = GetComponentInParent<UIMoneyContainer>();
        text = GetComponent<Text>();

        if (parent)
        {
            parent.DeclareThis(Label, this, transform.GetSiblingIndex());
        }
    }

    public void ChangeText(char newText)
    {
        text.text = newText.ToString();
    }
}
