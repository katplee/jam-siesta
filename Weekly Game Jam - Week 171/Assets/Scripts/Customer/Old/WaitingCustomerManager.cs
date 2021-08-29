using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingCustomerManager : Singleton<WaitingCustomerManager>
{
    /*
    public List<Transform> waitingList;

    private void Start()
    {
        foreach (Transform tr in transform)
        {
            waitingList.Add(tr);
        }
    }

    private void Update()
    {
        ChangeParenting();         
    }
    
    private void ChangeParenting()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            if (!Utilities._HasChildWithComponent<Customer>(transform.GetChild(i).gameObject))
            {
                GameObject customerObject;
                if (Utilities.HasChildWithComponent<Customer>(transform.GetChild(i + 1).gameObject, out customerObject))
                {
                    customerObject.transform.SetParent(transform.GetChild(i));
                    customerObject.GetComponent<Customer>().MoveCustomer(waitingList[i].name);
                }
                else
                {
                    return;
                }
            }
        }
    }
    */
}
