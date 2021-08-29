using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerLocation : Singleton<PlayerLocation>
{
    /*
    [SerializeField]
    private Tilemap playerTileMap;

    private Dictionary<string, Vector3Int> playerPosition;

    public enum playerLocation
    {
        PLAYER_DEFAULT,
        PLAYER_LOCATION,
        WAITING_LINE,
        SUITCASE_CABINET,
        PAJAMAS_CABINET,
        PLAYER_TOUR_CHECKPOINT,
        BED,
        DRESSER,
        CLEAN_SHEETS_CABINET,
        LAUNDRY,
        PAYING_LINE        
    }

    public string location;

    private void Update()
    {
        playerPosition = GameManager.Instance.playerPosition;
        Vector3Int transTilePos = playerTileMap.WorldToCell(transform.position);

        if (playerPosition.ContainsValue(transTilePos))
        {
            foreach (KeyValuePair<string, Vector3Int> pos in playerPosition)
            {
                if (pos.Value == transTilePos)
                {                  
                    location = pos.Key;
                }
            }
        }
    }
    */
}
