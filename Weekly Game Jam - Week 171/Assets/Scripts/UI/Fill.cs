using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<CanvasRenderer>().SetAlpha(0f);
    }
}
