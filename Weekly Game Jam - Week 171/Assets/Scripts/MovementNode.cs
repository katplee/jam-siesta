using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNode : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        MovementNodesArray.Instance.MovementArray.Add(this);
        GameManager.Instance.playerPosition.Add(gameObject.name, transform.localPosition);

        
    }    
}
