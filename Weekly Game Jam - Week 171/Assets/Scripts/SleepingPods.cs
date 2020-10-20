using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingPods : MonoBehaviour
{
    void Start()
    {
        SleepingPodsArray.Instance.PodsDictionary.Add(transform.parent.gameObject, gameObject.name);
    }
}
