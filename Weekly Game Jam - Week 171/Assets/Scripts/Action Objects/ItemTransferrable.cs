using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTransferrable : MonoBehaviour, IUserInterface
{
    private GameObject item;
    private Element owner;

    public string label
    {
        get { return name; }
    }

    private void Awake()
    {
        item = gameObject;
        SetOwner();
    }

    public void SetOwner()
    {
        owner = transform.parent.GetComponent<Element>();
    }

    public Element GetOwner()
    {
        return owner;
    }
}
