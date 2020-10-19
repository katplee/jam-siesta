using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNode : Singleton<MovementNode>
{

    // Start is called before the first frame update
    void Start()
    {
        MovementNodesArray.Instance.MovementArray.Add(this);
    }    
}
