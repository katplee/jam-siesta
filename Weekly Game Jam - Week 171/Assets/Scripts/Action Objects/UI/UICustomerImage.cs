using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustomerImage : UIObject
{
    private UIBedMonitor parent;
    private Image image;

    private void Awake()
    {
        parent = GetComponentInParent<UIBedMonitor>();
        image = GetComponent<Image>();

        if (parent)
        {
            parent.DeclareThis(Label, this);
        }
    }

    public void ChangeImage(Sprite customerImage)
    {
        image.sprite= customerImage;
    }
}
