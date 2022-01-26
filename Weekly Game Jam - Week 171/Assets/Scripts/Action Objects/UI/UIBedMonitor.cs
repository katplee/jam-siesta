using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBedMonitor : UIObject
{
    private UICleanBed bed = null;
    private UIWashPajamas pajamas = null;
    private UICustomerType ctype = null;

    private void Awake()
    {
        Debug.Log($"bed monitor: {transform.GetSiblingIndex()}");
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
