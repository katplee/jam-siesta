using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : Element
{
    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    public override Tilemap Tilemap { get; set; }

    private void Awake()
    {
        Tilemap = TilemapManager.Instance.playerTilemap;

    }
}
