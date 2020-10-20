using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMovementNodesArray : Singleton<CustomerMovementNodesArray>
{
    [SerializeField]
    private List<CustomerMovementNode> movementArray = new List<CustomerMovementNode>();

    public List<CustomerMovementNode> MovementArray
    {
        get { return movementArray; }
    }
}
