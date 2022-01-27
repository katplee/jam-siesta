using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBedMonitor : UIObject
{
    public static event Action<UIBedMonitor, int> OnBedSynchronization;

    private UICleanBed bed = null;
    private UIWashPajamas pajamas = null;
    private UICustomerType ctype = null;
    private int index = -1;

    private void Awake()
    {
        index = transform.GetSiblingIndex();
    }

    private void Start()
    {
        OnBedSynchronization?.Invoke(this, index);
    }

    public void ReceiveBool(string label, bool item)
    {
        switch (label)
        {
            case "clean_bed":
                //signal the player to clean the sheets; true = bed is dirty
                bed.ChangeOpacity(item);
                break;

            case "wash_pajamas":
                //signal the player to clean the sheets; true = bed is dirty
                pajamas.ChangeOpacity(item);
                break;

            default:
                break;
        }
    }

    private void SetBed(UICleanBed bed)
    {
        this.bed = bed;
    }

    private void SetPajamas(UIWashPajamas pajamas)
    {
        this.pajamas = pajamas;
    }

    public void DeclareThis<T>(string element, T UIObject)
        where T : UIObject
    {
        switch (element)
        {
            case "UICleanBed":
                SetBed(UIObject as UICleanBed);
                break;

            case "UIWashPajamas":
                SetPajamas(UIObject as UIWashPajamas);
                break;
        }
    }
}
