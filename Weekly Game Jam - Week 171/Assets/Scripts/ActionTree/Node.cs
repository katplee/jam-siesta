using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    protected NodeState _nodeState;

    public NodeState nodeState
    {
        get { return _nodeState; }
    }   

    public enum NodeState { SUCCESS, RUNNING, FAILURE }

    public abstract NodeState Evaluate();

}