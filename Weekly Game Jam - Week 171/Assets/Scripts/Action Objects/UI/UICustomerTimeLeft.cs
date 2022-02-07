using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustomerTimeLeft : UIObject
{
    private UIBedMonitor parent;
    private UICustomerTimeText timeText;
    private Text text;

    private void Awake()
    {
        parent = GetComponentInParent<UIBedMonitor>();
        timeText = parent.GetComponentInChildren<UICustomerTimeText>();

        text = GetComponent<Text>();

        if (parent)
        {
            parent.DeclareThis(Label, this);
        }
    }

    public void ChangeText(string customerTimeLeft)
    {
        text.text = customerTimeLeft;
        timeText.ChangeOpacity(true);
    }

    public void ResetText()
    {
        text.text = "";
        timeText.ChangeOpacity(false);
    }
}
