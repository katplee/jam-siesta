using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pod : MonoBehaviour
{
    private Bed bed = null;
    private Dresser dresser = null;

    private UIBedMonitor monitor = null;
    private int index = -1;

    private void Awake()
    {
        index = transform.GetSiblingIndex();
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SetBed(Bed bed)
    {
        this.bed = bed;
    }

    private void SetDresser(Dresser dresser)
    {
        this.dresser = dresser;
    }

    private void SetMonitor(UIBedMonitor monitor, int index)
    {
        if (index != this.index) { return; }
        this.monitor = monitor;
    }

    public void PassFloat(string label, float item)
    {
    }

    public void PassString(string label, string item)
    {

    }

    public void PassBool(string label, bool item)
    {
        monitor.ReceiveBool(label, item);
    }

    public void DeclareThis<T>(string element, T UIobject)
        where T : ItemClickable
    {
        switch (element)
        {
            case "Bed":
                SetBed(UIobject as Bed);
                break;

            case "Dresser":
                SetDresser(UIobject as Dresser);
                break;
        }
    }

    private void SubscribeEvents()
    {
        UIBedMonitor.OnBedSynchronization += SetMonitor;
    }

    private void UnsubscribeEvents()
    {
        UIBedMonitor.OnBedSynchronization -= SetMonitor;
    }
}
