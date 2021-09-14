using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingManager : Singleton<WaitingManager>
{
    public void SetQueueDestination(CustomerController caller)
    {
        foreach (Transform node in transform)
        {
            CustomerNode customerNode = null;
            if (node.TryGetComponent(out customerNode))
            {
                bool target = customerNode.GetOccupant() == caller.gameObject;
                if (target) { return; }
            }
            customerNode.Queue(caller);
        }
    }
}
