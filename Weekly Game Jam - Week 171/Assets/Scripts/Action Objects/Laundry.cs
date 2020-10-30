using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laundry : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    
    private void Update()
    {
        if(Vector3.Distance(playerTransform.position, transform.position) <= 1.5)
        {
            foreach(Transform t in playerTransform)
            {
                Destroy(t.gameObject);
            }
        }
    }
}
