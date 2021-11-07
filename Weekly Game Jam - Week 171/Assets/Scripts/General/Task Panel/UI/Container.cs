using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : UIObject
{
    private TaskPanel root = null;

    private void Awake()
    {
        if (transform.root.TryGetComponent(out root))
        {
            root.DeclareThis(Label, this);
        }    
    }
}
