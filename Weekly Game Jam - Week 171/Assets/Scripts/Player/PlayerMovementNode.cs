using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementNode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ADD NODE TO PLAYER POSITION DICTIONARY
        //POSITION IS SENT IN TERMS OF CELL POSITION
        Vector3Int nodeTilePos = Tilemaps.Instance.playerTileMap.WorldToCell(transform.position);
        GameManager.Instance.playerPosition.Add(gameObject.name, nodeTilePos);
    }    
}
