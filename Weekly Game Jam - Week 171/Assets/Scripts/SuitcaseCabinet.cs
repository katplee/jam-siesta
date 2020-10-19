using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitcaseCabinet : MonoBehaviour
{

    public GameManager.objectType objectType;

    private void Awake()
    {
        objectType = GameManager.objectType.SUITCASE_CABINET;
    }

    private void OnMouseDown()
    {
        if (!GameManager.Instance.SequenceRunning)
        {
            GameManager.Instance.clickRecord.Add(gameObject);
        }
    }
}
