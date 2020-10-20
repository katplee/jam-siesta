using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestination : Singleton<PlayerDestination>
{   
    public Vector3 Destination { get; set; }
    public MovementNode CurrentDestination { get; set; }   

    private void Start()
    {
        CurrentDestination = null;
        gameObject.GetComponent<PlayerController>().Destination = Destination;
    }

    private void Update()
    {
        CheckIfInDestination(Destination);
        Debug.Log("Player Destination: " + Destination);
    }

    public void CheckIfInDestination(Vector3 destination)
    {
        if (Vector3.Distance(Destination, transform.position) < 0.1f)
        {
            transform.position = Destination;
            PlayerController.Instance.InputVector = new Vector3(0f, 0f, 0f);
            Destroy(this);
        }
        else
        {
            SetDestinationByController(destination);
        }        

        /*
        if (Vector3.Distance(Destination, transform.position) < 0.1f)
        {
            onLastNode = false;
            transform.position = Destination;
            PlayerController.Instance.InputVector = new Vector3(0f, 0f, 0f);
            MovementDone = true;
            Destroy(this);      
        }
        else
        {
            SetDestinationByController(destination);
        }
        */
    }
    
    public void SetDestinationByController(Vector3 destination)
    {
        //Destination = destination;        

        Vector3 playerToIntDistance;
        float playerToIntDirection;
        Vector3 inputVector;

        playerToIntDistance = IntermediateNode() - transform.position;

        /*
        if (onLastNode)
        {            
            playerToIntDistance = Destination - transform.position;
            CurrentDestination = null;
        }
        else
        {
            playerToIntDistance = IntermediateNode() - transform.position;
        }
        */

        Debug.Log("playerToIntDistance.x = " + Mathf.Abs(playerToIntDistance.x) + "; " + "playerToIntDistance.y = " + Mathf.Abs(playerToIntDistance.y));

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
        Debug.Log(inputVector);
        PlayerController.Instance.InputVector = inputVector;
    }

    private Vector3 IntermediateNode()
    {
        Debug.Log("@ IntermediateNode()");
        Debug.Log("CurrentDestination: " + CurrentDestination);
               
        if (CurrentDestination == null || Vector3.Distance(CurrentDestination.transform.position, transform.position) < 0.2f)
        {
            if(CurrentDestination != null)
            {
                transform.position = CurrentDestination.transform.position;
                PlayerController.Instance.InputVector = new Vector3(0f, 0f, 0f);
            }            

            List<MovementNode> accessibleNodes = AccessibleNodes();

            MovementNode optimalNode = null;
            float shortestDistance = 0f;

            foreach (var node in accessibleNodes)
            {
                Vector3 playerToInt = node.transform.position - transform.position;
                Vector3 intToDest = Destination - node.transform.position;
                Debug.Log(node.transform.position + ", " + playerToInt.magnitude + ", " + intToDest.magnitude);

                if (optimalNode == null)
                {
                    shortestDistance = playerToInt.magnitude + intToDest.magnitude;
                    optimalNode = node;
                }

                if (playerToInt.magnitude + intToDest.magnitude < shortestDistance)
                {
                    shortestDistance = playerToInt.magnitude + intToDest.magnitude;
                    optimalNode = node;
                }

                Debug.Log(optimalNode);
            }
            CurrentDestination = optimalNode;
            Debug.Log(CurrentDestination.transform.position);
            return CurrentDestination.transform.position;
        }
        else
        {
            return CurrentDestination.transform.position;
        }        
    }    

    private List<MovementNode> AccessibleNodes()
    {
        Debug.Log("@ AccessibleNodes()");
        List<MovementNode> movementNodesArray = MovementNodesArray.Instance.MovementArray;
        List<MovementNode> accessibleNodes = new List<MovementNode>();
        MovementNode referenceNode = ReferenceNode();
        
        float x = referenceNode.transform.position.x;
        float y = referenceNode.transform.position.y;

        foreach(var node in movementNodesArray)
        {
            Debug.Log(node.transform.position);

            if(!(node.transform.position.x == x && node.transform.position.y == y))
            {
                if (node.transform.position.x == x || node.transform.position.y == y)
                {
                    accessibleNodes.Add(node);
                }
            }            
        }
        Debug.Log(accessibleNodes.Count);
        return accessibleNodes;        
    }

    private MovementNode ReferenceNode()
    {
        List<MovementNode> movementNodesArray = MovementNodesArray.Instance.MovementArray;

        MovementNode closestNode = null;
        float closestDistance = 0f;

        foreach (var node in movementNodesArray)
        {
            float currentDistance = Vector3.Distance(node.transform.position, transform.position);

            if (closestNode == null)
            {
                closestNode = node;
                closestDistance = currentDistance;
            }
            
            if ( currentDistance < closestDistance)
            {
                closestNode = node;
                closestDistance = currentDistance;
            }
        }
        Debug.Log("Reference node:" + closestNode.transform.position);
        return closestNode;
    }
}
