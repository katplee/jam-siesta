using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool SequenceRunning { get; set; }

    public List<GameObject> clickRecord;

    public Dictionary<string, Vector3> playerPosition = new Dictionary<string, Vector3>();

    public enum objectType
    {
        NONE,
        CUSTOMER,
        SUITCASE_CABINET,
        BED,        
        PAJAMAS,
        ALARM,
        LAUNDRY
    }   

    public GameObject clickedObject; //change this to private
    public objectType clickedObjectType; //change this to private

    private void Awake()
    {
        clickedObject = null;        
    }

    private void Update()
    {
        //CheckForRunning();
        ExecutePlayerAction();
        
    }        

    private void ExecutePlayerAction()
    {
        switch (clickedObjectType)
        {
            case (objectType.CUSTOMER):
            {                     
                    Customer.customerState customerState = clickedObject.GetComponent<Customer>().State;
                    switch (customerState)
                    {
                        case (Customer.customerState.WAITING):
                            SequenceRunning = true;
                            Player.Instance.CheckIfInDestination(playerPosition["Waiting Line"]);
                            break;
                    }                    
            }
                break;

            default:
                break;



        }
    }

    public void SetClickedObject(GameObject clickedObject, objectType clickedObjectType)
    {
        this.clickedObject = clickedObject;
        this.clickedObjectType = clickedObjectType;        
    }    
}
