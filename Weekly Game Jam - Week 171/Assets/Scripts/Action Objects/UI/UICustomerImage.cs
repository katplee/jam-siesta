﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustomerImage : UIObject
{
    private UIBedMonitor parent;
    private Image image;

    private const byte on_a = 75;
    private const byte off_a = 0;

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
        ChangeOpacity(true);
    }

    public void ChangeOpacity(bool on)
    {
        Color32 current = image.color;
        current.a = on ? on_a : off_a;
        image.color = current;
    }

    public void ResetImage()
    {
        image.sprite = null;
        ChangeOpacity(false);
    }
}
