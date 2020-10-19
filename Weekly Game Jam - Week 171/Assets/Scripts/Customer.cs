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
        SUITCASE_GIVEN,
        YES_PAJAMAS,
        YES_BED,
        SLEEPING,
        AWAKE,
        NO_PAJAMAS,
        YES_PAYMENT,
        YES_SUITCASE,        
    }

    private customerState[] statesOrder = new customerState[10]
        {customerState.WAITING, 
         customerState.GIVE_SUITCASE,
         customerState.SUITCASE_GIVEN,
         customerState.YES_PAJAMAS,
         customerState.YES_BED,
         customerState.SLEEPING,
         customerState.AWAKE,
         customerState.NO_PAJAMAS,
         customerState.YES_PAYMENT,
         customerState.YES_SUITCASE};

    public customerState State { get; set; }
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
            default:
                break;
        }
    }    

    private void OnMouseDown()
    {
        if (!GameManager.Instance.SequenceRunning)
        {
            GameManager.Instance.clickRecord.Add(gameObject);
            GameManager.Instance.SetClickedObject(gameObject, objectType);
        }        
    }

    private void OnMouseUp()
    {
        
    }

    private void ReadyToChangeState()
    {
        stateIndex = readyState;
        GameManager.Instance.clickRecord.Add(GameObject.Find("Click Divider"));

    }

    private bool AtWaiting()
    {
        if (Player.Instance.MovementDone)
        {
            if(GameManager.Instance.clickRecord[GameManager.Instance.clickRecord.Count - 1] == gameObject)
            {
                readyState++;
                return true;
            }            
        }
        return false;

    }

    private bool AtGiveSuitcase()
    {
        if(GameManager.Instance.clickRecord[GameManager.Instance.clickRecord.Count-1].name == "Suitcase Cabinet")
        {
            if(GameManager.Instance.clickRecord[GameManager.Instance.clickRecord.Count - 2] == gameObject)
            {
                readyState++;
                return true;
            }
        }
        return false;
    }
}
