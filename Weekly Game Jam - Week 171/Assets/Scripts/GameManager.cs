using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool SequenceRunning { get; set; }

    public List<GameObject> clickRecord;

    public GameObject trial;
    public Transform playerTransform;
    public Transform clickDividerTransform;

    public Dictionary<string, Vector3> playerPosition = new Dictionary<string, Vector3>();
    public Dictionary<string, Vector3> customerPosition = new Dictionary<string, Vector3>();

    public enum objectType
    {
        NONE,
        CUSTOMER,
        SUITCASE_CABINET,
        PAJAMAS_CABINET,
        BED,      
        PAJAMAS,
        DRESSER,
        LAUNDRY
    }   

    public GameObject clickedObject; //change this to private
    public objectType clickedObjectType; //change this to private

    private void Awake()
    {
        clickedObject = null;
        AddClickDivider();
    }

    private void Update()
    {
        //ExecutePlayerAction();
        if (Input.GetMouseButtonDown(0))
        {
            if(GameManager.Instance.playerTransform.GetComponent<PlayerDestination>() == null)
            {
                playerTransform.gameObject.AddComponent<PlayerDestination>();
                PlayerDestination.Instance.Destination = DecidePlayerDestination();
            }                    
        }        
    }        

    
    private Vector3 DecidePlayerDestination()
    {
        switch (clickedObjectType)
        {
            case (objectType.CUSTOMER):

                Customer.customerState customerState = clickedObject.GetComponent<Customer>().State;
                
                switch (customerState)
                {
                    case (Customer.customerState.WAITING):
                        PlayerLocation.Instance.location = PlayerLocation.playerLocation.WAITING_LINE;
                        return playerPosition["Waiting Line"];

                    case (Customer.customerState.GIVE_SUITCASE):
                        PlayerLocation.Instance.location = PlayerLocation.playerLocation.WAITING_LINE;
                        return playerPosition["Waiting Line"];

                    case (Customer.customerState.WAIT_PAJAMAS):
                        PlayerLocation.Instance.location = PlayerLocation.playerLocation.WAITING_LINE;
                        return playerPosition["Waiting Line"];

                    case (Customer.customerState.WAIT_TOUR):
                        PlayerLocation.Instance.location = PlayerLocation.playerLocation.TOUR_CHECKPOINT;
                        return playerPosition["Player Tour Checkpoint"];
                }                
                
                break;

            case (objectType.SUITCASE_CABINET):
                PlayerLocation.Instance.location = PlayerLocation.playerLocation.SUITCASE_CABINET;
                return playerPosition["Suitcase Cabinet"];

            case (objectType.PAJAMAS_CABINET):
                PlayerLocation.Instance.location = PlayerLocation.playerLocation.PAJAMAS_CABINET;
                return playerPosition["Pajamas Cabinet"];

            case (objectType.BED):
                PlayerLocation.Instance.location = PlayerLocation.playerLocation.BEDSIDE;
                return trial.transform.position;
                //return playerPosition["Pajamas Cabinet"];
        }
        return playerPosition["Player Default"];
    }
    
    public void AddClickDivider()
    {
        clickRecord.Add(clickDividerTransform.gameObject);
    }
    

    public void SetClickedObject(GameObject clickedObject, objectType clickedObjectType)
    {
        this.clickedObject = clickedObject;
        this.clickedObjectType = clickedObjectType;        
    }    
}
