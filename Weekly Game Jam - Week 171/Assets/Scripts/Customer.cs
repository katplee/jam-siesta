using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : Singleton<Customer>
{
    public GameManager.objectType objectType;

    public enum customerState
    {
        WAITING,
        GIVE_SUITCASE,
        WAIT_PAJAMAS,
        WAIT_TOUR,
        YES_BED,        
        AWAKE,
        NO_PAJAMAS,
        YES_PAYMENT,
        YES_SUITCASE,        
    }

    private customerState[] statesOrder = new customerState[9]
        {customerState.WAITING, 
         customerState.GIVE_SUITCASE,
         customerState.WAIT_PAJAMAS,
         customerState.WAIT_TOUR,
         customerState.YES_BED,
         customerState.AWAKE,
         customerState.NO_PAJAMAS,
         customerState.YES_PAYMENT,
         customerState.YES_SUITCASE};

    public customerState State { get; set; }

    List<GameObject> clickRecord;

    public int stateIndex = 0;
    public int readyState = 0;

    private int patientMeter; //patience tracker
    private int patienceDecrease; //rate patience decreases
    private int stayThreshold; //patience level less of which customer leaves

    private float leaveTime; //the time until customer leaves
    private float sleepCountdown; //the length of sleep remaining    

    private int violationMeter; //violation counter
    private int tipMeter; //tip tracker   

    private void Awake()
    {
        objectType = GameManager.objectType.CUSTOMER;
    }

    private void Start()
    {
        clickRecord = GameManager.Instance.clickRecord;
    }
    private void Update()
    {
        State = statesOrder[stateIndex];

        switch (State)
        {
            case (customerState.WAITING):
                if (AtWaiting()) { ReadyToChangeState(); }
                break;

            case (customerState.GIVE_SUITCASE):
                if (AtGiveSuitcase()) { ReadyToChangeState(); }
                break;

            case (customerState.WAIT_PAJAMAS):
                if (AtWaitPajamas()) { ReadyToChangeState(); }
                break;

            case (customerState.WAIT_TOUR):
                if (AtWaitTour()) { ReadyToChangeState(); }
                break;

            case (customerState.YES_BED):
                if (AtYesBed()) { ReadyToChangeState(); }
                break;

            default:
                break;
        }
    }    

    private void OnMouseDown()
    {
        if (GameManager.Instance.playerTransform.GetComponent<PlayerDestination>() == null)
        {
            clickRecord.Add(gameObject);
            GameManager.Instance.SetClickedObject(gameObject, objectType);            
        }        
    }

    private void ReadyToChangeState()
    {
        stateIndex = readyState;
        GameManager.Instance.AddClickDivider();
    }

    private bool AtWaiting()
    {
        if (GameManager.Instance.playerTransform.GetComponent<PlayerDestination>() == null)
        {
            if(clickRecord[clickRecord.Count - 1] == gameObject)
            {
                readyState++;
                return true;
            }            
        }
        return false;
    }

    private bool AtGiveSuitcase()
    {
        if(clickRecord[clickRecord.Count-1].name == "Suitcase Cabinet")
        {
            if(clickRecord[clickRecord.Count - 2] == gameObject)
            {
                readyState++;
                return true;
            }
        }
        return false;
    }

    private bool AtWaitPajamas()
    {
        if (clickRecord[clickRecord.Count - 1] == gameObject)
        {
            if (clickRecord[clickRecord.Count - 2].name == "Pajamas Cabinet")
            {
                readyState++;
                return true;
            }
        }
        return false;
    }

    private bool AtWaitTour()
    {
        if (clickRecord[clickRecord.Count - 1] == gameObject)
        {
            if (gameObject.GetComponent<CustomerDestination>() == null)
            {
                CustomerDestination customer = gameObject.AddComponent<CustomerDestination>();
                customer.Destination = GameManager.Instance.customerPosition["Customer Tour Checkpoint"];
            }

            if (Mathf.Abs(transform.position.x - PlayerLocation.Instance.transform.position.x) <= 1f)
            {
                readyState++;
                return true;
            }
        }
        return false;
    }

    private bool AtYesBed()
    {
        if (clickRecord[clickRecord.Count - 1].tag == "Bed")
        {
            if (gameObject.GetComponent<CustomerDestination>() == null)
            {
                CustomerDestination customer = gameObject.AddComponent<CustomerDestination>();
                customer.Destination = GameManager.Instance.customerPosition[clickRecord[clickRecord.Count - 1].name + " Node"];
            }

            if (Mathf.Abs(transform.position.x - clickRecord[clickRecord.Count - 1].transform.position.x) <= 1f)
            {
                readyState++;
                return true;
            }
        }
        return false;
    }
}
