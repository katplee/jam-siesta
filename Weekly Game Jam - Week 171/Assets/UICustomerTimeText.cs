using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustomerTimeText : UIObject
{
    private UIBedMonitor parent;
    private Text text;

    private const byte on_a = 255;
    private const byte off_a = 0;

    private void Awake()
    {
        parent = GetComponentInParent<UIBedMonitor>();
        text = GetComponent<Text>();

        if (parent)
        {
            parent.DeclareThis(Label, this);
        }
    }

    public void ChangeOpacity(bool on)
    {
        Color32 current = text.color;
        current.a = on ? on_a : off_a;
        text.color = current;
    }
}

