using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustomerType : UIObject
{
    private UIBedMonitor parent;
    private Text text;

    private void Awake()
    {
        parent = GetComponentInParent<UIBedMonitor>();
        text = GetComponent<Text>();

        if (parent)
        {
            parent.DeclareThis(Label, this);
        }
    }

    public void ChangeText(string customerType)
    {
        text.text = customerType;
    }

    public void EmptyText()
    {
        text.text = "[VACANT]";
    }
}
