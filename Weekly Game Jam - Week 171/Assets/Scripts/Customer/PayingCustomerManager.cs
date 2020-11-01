using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayingCustomerManager : Singleton<PayingCustomerManager>
{
    public List<Transform> payingList;

    private void Start()
    {
        foreach(Transform tr in transform)
        {
            payingList.Add(tr);
        }
    }

}
