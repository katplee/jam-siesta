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

    /*
    public void OnTransfer<T>(Element receiver, T2 transferLocation)
        where T1 : MNode
    {
        //transfer location is the node on which the receiver must be in to be able to receiver the item
        Vector3Int position;
    }
    */
}
