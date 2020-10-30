using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingCustomerManager : Singleton<WaitingCustomerManager>
{
    [SerializeField]
    private List<WaitingNode> waitingArray = new List<WaitingNode>();

    public List<WaitingNode> WaitingArray
    {
        get { return waitingArray; }
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!Utilities._HasChildWithComponent<Customer>(transform.GetChild(0).gameObject))
        {
            //ChangeParenting();
        }     
        
    }
    
    private void ChangeParenting()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!Utilities._HasChildWithComponent<Customer>(transform.GetChild(i).gameObject))
            {
                GameObject customerObject;
                if (Utilities.HasChildWithComponent<Customer>(transform.GetChild(i + 1).gameObject, out customerObject))
                {
                    customerObject.transform.SetParent(transform.GetChild(i));                    
                }
                else
                {
                    return;
                }
            }
        }
    }
}
