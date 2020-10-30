using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingNode : MonoBehaviour
{
    void Start()
    {
        WaitingCustomerManager.Instance.WaitingArray.Add(this);
    }
    
    private void Update()
    {
        
    }

    
    
}
