using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PajamasCabinet : MonoBehaviour
{

    public GameManager.objectType objectType;

    private void Awake()
    {
        objectType = GameManager.objectType.PAJAMAS_CABINET;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.playerTransform.GetComponent<PlayerDestination>() == null)
        {
            GameManager.Instance.clickRecord.Add(gameObject);
            GameManager.Instance.SetClickedObject(gameObject, objectType);
        }
    }
}
