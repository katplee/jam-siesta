using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotTemplate : UIObject
{
    private TaskPanel root = null;

    private void Awake()
    {
        if (transform.root.TryGetComponent(out root))
        {
            if (!transform.parent.GetComponent<Container>())
            {
                root.DeclareThis(Label, this);
                transform.localScale = Vector3.zero;
            }
        }
    }
}
