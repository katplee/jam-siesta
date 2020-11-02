using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
    public customerState State { get; set; }
    public enum customerState
    {
        WAITING,
        GIVE_SUITCASE,
        WAIT_PAJAMAS,
        WAIT_TOUR,
        YES_BED,
        CHANGED_PAJAMAS,
        WAIT_WAKEUP,
        CHANGED_CLOTHES,
        PAYING,
        WAIT_CASHIER,
        RECEIVE_SUITCASE,
        LEAVING
    }

    private customerState[] statesOrder = new customerState[12]
        {customerState.WAITING, 
         customerState.GIVE_SUITCASE,
         customerState.WAIT_PAJAMAS,
         customerState.WAIT_TOUR,
         customerState.YES_BED,
         customerState.CHANGED_PAJAMAS,
         customerState.WAIT_WAKEUP,
         customerState.CHANGED_CLOTHES,
         customerState.PAYING,
         customerState.WAIT_CASHIER,
         customerState.RECEIVE_SUITCASE,
         customerState.LEAVING};

    public customerType type; //turn into a get-set       
    public enum customerType
    {
        PARENT,
        SALARY_MAN,
        STUDENT
    }

    List<GameObject> clickRecord;    
    private GameObject custInPlayContainer;
    private Tilemap customerTileMap;

    public GameObject bedObject;    
    [SerializeField]
    private GameObject dirtySheetsObject;

    public int stateIndex = 0;
    public int readyState = 0;

    private int patientMeter; //patience tracker
    private int patienceDecrease; //rate patience decreases
    private int stayThreshold; //patience level less of which customer leaves

    private float blinkingLength = 2f;
    public float blinkingTime = 0f;

    public bool mustWakeUp;
    public bool countdownOn;
    public float sleepNeeded; //the length of sleep remaining

    private float violationTimer;
    [SerializeField]
    private float violationMeter; //violation counter
    private int tipMeter; //tip tracker   

    private void Awake()
    {        
        int randomize = Random.Range(0, 2);
        type = (customerType)randomize;
    }

    private void Start()
    {   
        clickRecord = GameManager.Instance.clickRecord;
        custInPlayContainer = Containers.Instance.custInPlayContainer;
        customerTileMap = Tilemaps.Instance.customerTileMap;
        sleepNeeded = SetSleepTime();
    }

    private void Update()
    {
        State = statesOrder[stateIndex];   
        
        CustomerDestination customerSetDestination = gameObject.GetComponent<CustomerDestination>();
        PlayerDestination playerSetDestination = GameManager.Instance.playerTransform.GetComponent<PlayerDestination>();
        Transform playerTransform = PlayerLocation.Instance.transform;
        PlayerLocation playerInstance = PlayerLocation.Instance;
        GameObject lastClick = clickRecord[clickRecord.Count - 1];
        GameObject previousClick = clickRecord[clickRecord.Count - 2];               

        switch (State)
        {
            case (customerState.WAITING):
                if (AtWaiting(playerSetDestination, lastClick)) { ReadyToChangeState(); }
                break;

            case (customerState.GIVE_SUITCASE):
                if (AtGiveSuitcase(playerSetDestination, lastClick, previousClick)) { ReadyToChangeState(); }
                break;

            case (customerState.WAIT_PAJAMAS):
                if (AtWaitPajamas(playerSetDestination, lastClick, previousClick)) { ReadyToChangeState(); }
                break;

            case (customerState.WAIT_TOUR):
                if (AtWaitTour(playerTransform, customerSetDestination, lastClick)) { ReadyToChangeState(); }
                break;

            case (customerState.YES_BED):
                if (AtYesBed(customerSetDestination, lastClick, previousClick)) { ReadyToChangeState(); }
                break;

            case (customerState.CHANGED_PAJAMAS):
                if (AtChangedPajamas()) { ReadyToChangeState(); }
                break;

            case (customerState.WAIT_WAKEUP):
                if (AtWaitWakeup(playerInstance, lastClick, previousClick)) { ReadyToChangeState(); }
                break;
            
            case (customerState.CHANGED_CLOTHES):
                if (AtChangedClothes()) { ReadyToChangeState(); }
                break;

            case (customerState.PAYING):
                if (AtPaying(customerSetDestination)) { ReadyToChangeState(); }
                break;

            case (customerState.WAIT_CASHIER):
                if (AtWaitCashier(playerInstance, playerSetDestination, lastClick)) { ReadyToChangeState(); }
                break;

            case (customerState.RECEIVE_SUITCASE):
                if (AtReceiveSuitcase(playerSetDestination, lastClick, previousClick)) { ReadyToChangeState(); }
                break;

            case (customerState.LEAVING):
                if (AtLeaving()) { Destroy(gameObject); }
                break;

            default:
                break;
        }
    }    

    private bool AtWaiting(PlayerDestination playerSetDestination, GameObject lastClick)
    {
        if (playerSetDestination == null)
        {
            if (lastClick == gameObject)
            {
                readyState++;
                return true;
            }            
        }
        return false;
    }

    private bool AtGiveSuitcase(PlayerDestination playerSetDestination, GameObject lastClick, GameObject previousClick)
    {
        if(playerSetDestination == null)
        {
            if(previousClick == gameObject)
            {
                if(lastClick.name == "SUITCASE_CABINET")
                {
                    readyState++;
                    return true;
                }
            }
        }
        return false;
    }

    private bool AtWaitPajamas(PlayerDestination playerSetDestination, GameObject lastClick, GameObject previousClick)
    {
        if (playerSetDestination == null) 
        {
            if (previousClick.name == "PAJAMAS_CABINET")
            {
                if (lastClick == gameObject)
                {
                    readyState++;
                    return true;
                }
            }
        }
        return false;
    }

    private bool AtWaitTour(Transform playerTransform, CustomerDestination customerSetDestination, GameObject lastClick)
    {
        if (customerSetDestination == null) 
        {
            if (lastClick == gameObject)
            {
                //UPDATES THE CUSTOMER'S TRANSFORM CONTAINER
                transform.SetParent(custInPlayContainer.transform);
                //MOVES THE CUSTOMER TO THE TOUR CHECKPOINT NODE
                MoveCustomer("CUSTOMER_TOUR_CHECKPOINT");                
            }

            if (Vector3.Distance(transform.position, playerTransform.position) <= 1f)
            {
                readyState++;
                return true;
            }
        }
        return false;
    }

    private bool AtYesBed(CustomerDestination customerSetDestination, GameObject lastClick, GameObject previousClick)
    {
        if (customerSetDestination == null) 
        {
            if(previousClick == gameObject)
            {
                if (lastClick.name.StartsWith("BED"))
                {
                    if (lastClick.GetComponent<Bed>().IsAvailable)
                    {
                        //STORES THE BED USED BY THE CUSTOMER
                        bedObject = lastClick;
                        //MOVES THE CUSTOMER TO THE NODE NEXT TO THE BED
                        MoveCustomer(lastClick.name);
                    }

                    if (Vector3.Distance(transform.position, lastClick.transform.position) <= 1.5f)
                    {
                        readyState++;
                        return true;
                    }
                }
            }
        }
        return false;
    }    

    private bool AtChangedPajamas()
    {
        Debug.Log("Changes to pajamas animation");

        if (DoChangingAnimation())
        {
            readyState++;
            return true;
        }
        return false;
    }

    private bool AtWaitWakeup(PlayerLocation playerInstance, GameObject lastClick, GameObject previousClick)
    {
        RunSleepSequence();

        if (lastClick == bedObject)
        {
            string dresserName = bedObject.name.Replace("BED", "DRESSER");

            if (previousClick.name == dresserName)
            {
                if (playerInstance.location == bedObject.name)
                {
                    RunWakeUpSequence();
                    readyState++;
                    return true;
                }
            }
        }
        return false;
    }       

    private bool AtChangedClothes()
    {
        Debug.Log("Changes to original clothes animation");

        if (DoChangingAnimation())
        {
            readyState++;
            return true;
        }
        return false;
    }

    private bool AtPaying(CustomerDestination customerSetDestination)
    {       
        if (customerSetDestination == null)
        {
            //Vector3Int destinationVector;
            //Vector3 trWorldPos = transform.position;
            //Vector3Int trTilePos = customerTileMap.WorldToCell(trWorldPos);

            if (!transform.parent.name.StartsWith("PAYING_LINE"))
            {
                MoveCustomer(ChoosePayingPoint());
            }

            if (transform.parent.name == "PAYING_LINE_1")
            {
                readyState++;
                return true;
            }
        }        
        return false;
    }    

    private bool AtWaitCashier(PlayerLocation playerInstance, PlayerDestination playerSetDestination, GameObject lastClick)
    {
        if (playerSetDestination == null)
        {
            if (lastClick == gameObject)
            {
                if (playerInstance.location == "PAYING_LINE")
                {
                    readyState++;
                    return true;
                }                    
            }
        }
        return false;
    }

    private bool AtReceiveSuitcase(PlayerDestination playerSetDestination, GameObject lastClick, GameObject previousClick)
    {
        if (playerSetDestination == null)
        {
            if (previousClick.name == "SUITCASE_CABINET")
            {
                if (lastClick == gameObject)
                {
                    readyState++;
                    return true;
                }
            }
        }
        return false;
    }

    private bool AtLeaving()
    {
        Debug.Log("Do animation.");
        return true;
    }

    #region BASIC METHODS

    private void ReadyToChangeState()
    {
        stateIndex = readyState;
        GameManager.Instance.AddClickDivider();
    }

    private float SetSleepTime()
    {
        if (type == customerType.PARENT){ return 20f; }
        if (type == customerType.SALARY_MAN) { return 15f; }        
        return 10f;
    }

    private void MoveCustomer(string customerDestination, out Vector3Int destinationVector)
    {
        CustomerDestination customer = gameObject.AddComponent<CustomerDestination>();
        destinationVector = GameManager.Instance.customerPosition[customerDestination];
        customer.Destination = destinationVector;
    }

    public void MoveCustomer(string customerDestination)
    {
        MoveCustomer(customerDestination, out _);
    }

    private string ChoosePayingPoint()
    {
        List<Transform> payingList = PayingCustomerManager.Instance.payingList;
        foreach(Transform tr in payingList)
        {
            if (tr.childCount == 0)
            {
                transform.SetParent(tr);
                return tr.name;
            }
        }
        return bedObject.name;
    }

    private bool DoChangingAnimation()
    {
        blinkingTime += Time.deltaTime;
        
        if (gameObject.GetComponent<SpriteRenderer>().enabled == false)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (Mathf.Clamp(blinkingTime, 0f, blinkingLength) == blinkingLength)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            blinkingTime = 0f;
            return true;
        }
        return false;
    }

    private void RunSleepSequence()
    {
        //SET BED AS PARENT TRANSFORM
        transform.SetParent(bedObject.transform);
        //HIDE TRANSFORM        
        if (transform.parent == bedObject.transform)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        
        //ASSESS VIOLATIONS
        sleepNeeded = CountMeritDemerit();
        Debug.Log("Customer sleeping animation");        
    }

    private float CountMeritDemerit()
    {
        countdownOn = true;
        sleepNeeded -= Time.deltaTime;
        mustWakeUp = (Mathf.Sign(sleepNeeded) == -1) ? true : false;

        violationMeter = Mathf.Abs(Mathf.Ceil(sleepNeeded / 4f));

        return sleepNeeded;
    }

    private void RunWakeUpSequence()
    {
        transform.SetParent(custInPlayContainer.transform);
                
        if (transform.parent == custInPlayContainer.transform)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        
        StopMeritDemerit();
        GameObject go = Instantiate(dirtySheetsObject, bedObject.transform);
        go.name = dirtySheetsObject.name;
        
    }

    private void StopMeritDemerit()
    {
        countdownOn = false;        
        mustWakeUp = false;        
    }

    #endregion
}
