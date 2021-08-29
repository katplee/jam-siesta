using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayingCustomerManager : Singleton<PayingCustomerManager>
{
    /*
    public List<Transform> payingList;

    private void Start()
    {
        foreach(Transform tr in transform)
        {
            payingList.Add(tr);
        }
    }

    private void Update()
    {
        ChangeParenting();
    }

    private void ChangeParenting()
    {
        for (int i = 0; i < payingList.Count; i++)
        {
            if (!Utilities._HasChildWithComponent<Customer>(transform.GetChild(i).gameObject))
            {
                GameObject customerObject;
                if (Utilities.HasChildWithComponent<Customer>(transform.GetChild(i + 1).gameObject, out customerObject))
                {
                    customerObject.transform.SetParent(transform.GetChild(i));
                    customerObject.GetComponent<Customer>().MoveCustomer(payingList[i].name);
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
