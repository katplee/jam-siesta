using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pod : MonoBehaviour
{
    private Bed bed = null;
    private Dresser dresser = null;

    private UIBedMonitor monitor = null;

    private void Awake()
    {
        Debug.Log($"pod: {transform.GetSiblingIndex()}");
    }

    private void SetBed(Bed bed)
    {
        this.bed = bed;
    }

    private void SetDresser(Dresser dresser)
    {
        this.dresser = dresser;
    }

    private void SetMonitor(UIBedMonitor monitor)
    {
        this.monitor = monitor;
    }

    public void DeclareThis<T>(string element, T UIobject)
    {
        switch (element)
        {
            case "Bed":
                SetBed(UIobject as Bed);
                break;

            case "Dresser":
                SetDresser(UIobject as Dresser);
                break;

            case "UIBedMonitor":
                SetMonitor(UIobject as UIBedMonitor);
                break;
        }
    }
}
