using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemClickable : MonoBehaviour, IUserInterface
{
    public string label
    {
        get { return name; }
    }

    public void OnClick()
    {
        PlayerController.Instance.AnimatePlayer(transform);
        //note: position conversion to cell position will be done inside the move player method
    }
}
