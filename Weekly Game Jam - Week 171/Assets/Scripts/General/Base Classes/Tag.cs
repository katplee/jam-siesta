using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag : MonoBehaviour, IUserInterface
{
    public string label
    {
        get { return GetType().Name; }
    }
}
