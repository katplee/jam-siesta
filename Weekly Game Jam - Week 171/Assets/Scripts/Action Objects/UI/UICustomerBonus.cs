using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustomerBonus : UIObject
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

    public void ChangeText(string customerBonus)
    {
        text.text = customerBonus;
    }
}
