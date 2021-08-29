using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDestination : Singleton<PlayerDestination>
{
    /*
    public Vector3Int Destination { get; set; }        
    public PlayerController Controller { get; set; }
    private Node current;
    private Vector3Int startPos;
    private Tilemap playerTileMap;
    private HashSet<Node> openList;
    private HashSet<Node> closedList;
    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    private Stack<Vector3Int> finalPath;

    private void Awake()
    {
        Controller = gameObject.GetComponent<PlayerController>();
    }

    private void Start()
    {
        playerTileMap = Tilemaps.Instance.playerTileMap;
        current = null;
        finalPath = null;
        startPos = playerTileMap.WorldToCell(transform.position);
        Controller.Destination = Destination;
    }

    private void Update()
    {
        //Checks every frame if the player is already in the destination.

        Vector3 trWorldPos = transform.position;
        Vector3Int trTilePos = playerTileMap.WorldToCell(trWorldPos);

        CheckIfInDestination(Destination, trTilePos);
    }

    public void CheckIfInDestination(Vector3Int destination, Vector3Int transformPosition)
    {
        //Runs this method to check if the player is already in the destination.
        //If the player is already in the destination, the input vector is set to Vector3.zero.
        // And the PlayerDestination script parameter is deleted.

        //If the player is not in the destination, the player is moved.

        if (Vector3Int.Distance(destination, transformPosition) < 0.1f)
        {
            Controller.InputVector = new Vector3(0f, 0f, 0f);
            Destroy(this);
        }
        else
        {
            SetDestinationByController(destination, transformPosition);
        }
    }

    public void SetDestinationByController(Vector3Int destination, Vector3Int transformPosition)
    {
        //All but the last computed destination are called intermediate destination nodes.
        //So the input vector is always of the form (+/-1, 0, 0) or (0, +/-1, 0) - meaning the player will only move one square at a time.
        //The player is restricted to only horizontal and vertical steps.

        //To determine input vector that will be sent to the controller, the first intermediate destination is computed.
        //The computed intermediate destination will then be used in the computation of the input vector.

        Vector3Int playerToIntDistance;
        float playerToIntDirection;
        Vector3 inputVector;

        playerToIntDistance = IntermediateDest(destination, transformPosition) - transformPosition;

        if (Mathf.Abs(playerToIntDistance.x) > Mathf.Abs(playerToIntDistance.y))
        {
            playerToIntDirection = Mathf.Sign(playerToIntDistance.x);
            inputVector = new Vector3(playerToIntDirection, 0, 0);
        }
        else
        {
            playerToIntDirection = Mathf.Sign(playerToIntDistance.y);
            inputVector = new Vector3(0, playerToIntDirection, 0);
        }
        Controller.InputVector = inputVector;
    }

    private Vector3Int IntermediateDest(Vector3Int destination, Vector3Int transformPosition)
    {
        //In the computation of the intermediate destination, the A-Star pathfinding algorith is used.
        //Upon completion of the algorithm, a final path will be returned.

        //If there is no path, destination will be returned and may be faulty..

        AstarAlgorithm();

        if (finalPath != null && finalPath.Count > 0)
        {
            if (Vector3.Distance(transformPosition, finalPath.Peek()) < 0.05f)
            {
                finalPath.Pop();
            }
            return finalPath.Peek();            
        }
        else
        {
            return destination;
        }
    }
    */
}
