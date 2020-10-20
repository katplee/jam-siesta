using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMovementNode : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        CustomerMovementNodesArray.Instance.MovementArray.Add(this);
        GameManager.Instance.customerPosition.Add(gameObject.name, transform.position);
    }    
}
