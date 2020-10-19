using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : Singleton<MovementNode>
{

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.playerPosition.Add(this.gameObject.name, transform.position);
        Debug.Log(this.gameObject.name);
    }
}
