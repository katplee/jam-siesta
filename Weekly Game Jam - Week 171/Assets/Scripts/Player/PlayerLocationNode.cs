using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocationNode : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    private float speed = 5.0f;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
    }
}
