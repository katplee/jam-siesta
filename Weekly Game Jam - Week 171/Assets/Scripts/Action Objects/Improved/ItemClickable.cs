using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemClickable : MonoBehaviour, IUserInterface
{
    private Transform node = null;

    public string label
    {
        get { return name; }
    }

    private void Awake()
    {
        node = transform.GetChild(0);
    }

    public void OnClick()
    {
        PlayerController.Instance.TransportPlayer(node);
        //note: position conversion to cell position will be done inside the move player method
    }
}
