using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNodesArray : Singleton<MovementNodesArray>
{
    [SerializeField]
    private List<MovementNode> movementArray = new List<MovementNode>();

    public List<MovementNode> MovementArray
    {
        get { return movementArray; }
    }
}
