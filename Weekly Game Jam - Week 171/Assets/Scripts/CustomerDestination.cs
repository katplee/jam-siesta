using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerDestination : MonoBehaviour
{   
    public Vector3 Destination { get; set; }
    public CustomerMovementNode CurrentDestination { get; set; }
    public CustomerController Controller { get; set; }

    private void Awake()
    {
        Controller = gameObject.GetComponent<CustomerController>();        
    }

    private void Start()
    {
        CurrentDestination = null;
        Controller.Destination = Destination;
    }

    private void Update()
    {
        CheckIfInDestination(Destination);
        Debug.Log("Customer Destination: " + Destination);
    }

    public void CheckIfInDestination(Vector3 destination)
    {
        if (Vector3.Distance(Destination, transform.position) < 0.1f)
        {
            transform.position = Destination;
            Controller.InputVector = new Vector3(0f, 0f, 0f);
            Controller.hasFinishedSequence = true;
            Destroy(this);
        }
        else
        {
            SetDestinationByController(destination);
        }
    }
    
    public void SetDestinationByController(Vector3 destination)
    {
        Vector3 playerToIntDistance;
        float playerToIntDirection;
        Vector3 inputVector;

        playerToIntDistance = IntermediateNode() - transform.position;

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
        Controller.InputVector = inputVector;
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
                Controller.InputVector = new Vector3(0f, 0f, 0f);
            }            

            List<CustomerMovementNode> accessibleNodes = AccessibleNodes();

            CustomerMovementNode optimalNode = null;
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

    private List<CustomerMovementNode> AccessibleNodes()
    {
        Debug.Log("@ AccessibleNodes()");
        List<CustomerMovementNode> movementNodesArray = CustomerMovementNodesArray.Instance.MovementArray;
        List<CustomerMovementNode> accessibleNodes = new List<CustomerMovementNode>();
        CustomerMovementNode referenceNode = ReferenceNode();
        
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

    private CustomerMovementNode ReferenceNode()
    {
        List<CustomerMovementNode> movementNodesArray = CustomerMovementNodesArray.Instance.MovementArray;

        CustomerMovementNode closestNode = null;
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
