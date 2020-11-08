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
    private Tilemap customerTileMap;
    private GameObject custInPlayContainer;
    private GameObject UICanvas;
    private GameObject patienceObject;
    private GameObject patiencePF;
    private GameObject fillPF;
    private PatienceCountdownUI patienceBar = null;
    public GameObject bedObject;
    private GameObject dirtySheetsPF;

    public int stateIndex = 0;
    public int readyState = 0;

    [SerializeField]
    private float waitingPatience;
    [SerializeField]
    private float patienceLeft; //patience tracker

    private float blinkingLength = 2f;
    public float blinkingTime = 0f;

    public bool mustWakeUp;
    public bool countdownOn;
    public float sleepNeeded; //the length of sleep remaining

    [SerializeField]
    private float payment;
    private float violationCounter;
    [SerializeField]
    private int tipCounter; //tip tracker

    private float free = 0f;
    private float baggageDeposit = 50f;
    private float pajamasRent = 50f;
    private float bedRent = 100f;
    private float alarmService = 50f;
    private float baggageClaim = 30f;
       

    private void Awake()
    {        
        int randomize = Random.Range(0, 2);
        type = (customerType)randomize;
    }

    private void Start()
    {
        clickRecord = GameManager.Instance.clickRecord;
        customerTileMap = Tilemaps.Instance.customerTileMap;
        custInPlayContainer = Containers.Instance.custInPlayContainer;        
        SetCustomerTimers();
        UICanvas = Containers.Instance.UICanvas;
        patiencePF = Prefabs.Instance.patiencePF;
        fillPF = Prefabs.Instance.fillPF;
        dirtySheetsPF = Prefabs.Instance.dirtySheetsPF;
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
                if (AtWaiting(playerSetDestination)) { stateIndex = readyState; }
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

    private bool AtWaiting(PlayerDestination playerSetDestination)
    {
        RunWaitingSequence();
        CheckForLeave();

        if (playerSetDestination == null)
        {
            Vector3 trWorldPos = transform.position;
            Vector3Int trTilePos = customerTileMap.WorldToCell(trWorldPos);

            if (transform.parent.name == "WAITING_LINE_1")
            {
                if (Vector3.Distance(trTilePos, FindCustomerDest("WAITING_LINE_1")) <= 0.2f)
                {
                    readyState++;
                    return true;
                }
            }            
        }
        return false;
    }

    private bool AtGiveSuitcase(PlayerDestination playerSetDestination, GameObject lastClick, GameObject previousClick)
    {
        if (patienceObject)
        {
            RunWaitingSequence();
            CheckForLeave();

            if(playerSetDestination == null)
            {
                if (lastClick == gameObject)
                {
                    DestroyPatienceBar();
                    patienceLeft = -11f;
                }
            }            
        }        
        else 
        {
            Countdown(ref patienceLeft, out bool bonus, out bool penalty, minValueBeforeStop: -10);

            if (playerSetDestination == null)
            {
                if (previousClick == gameObject)
                {
                    if (lastClick.name == "SUITCASE_CABINET")
                    {
                        AccountPayment(bonus, penalty, baggageDeposit);
                        patienceLeft = -11f; //restart patienceLeft
                        readyState++;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool AtWaitPajamas(PlayerDestination playerSetDestination, GameObject lastClick, GameObject previousClick)
    {
        Countdown(ref patienceLeft, out bool bonus, out bool penalty, minValueBeforeStop: -10);

        if (playerSetDestination == null) 
        {
            if (previousClick.name == "PAJAMAS_CABINET")
            {
                if (lastClick == gameObject)
                {
                    AccountPayment(bonus, penalty, pajamasRent);
                    patienceLeft = -11f; //restart patienceLeft
                    readyState++;
                    return true;
                }
            }
        }
        return false;
    }

    private bool AtWaitTour(Transform playerTransform, CustomerDestination customerSetDestination, GameObject lastClick)
    {
        Countdown(ref patienceLeft, out bool bonus, out bool penalty, minValueBeforeStop: -10);

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
                AccountPayment(bonus, penalty, free);
                patienceLeft = -11f; //restart patienceLeft
                readyState++;
                return true;
            }
        }
        return false;
    }

    private bool AtYesBed(CustomerDestination customerSetDestination, GameObject lastClick, GameObject previousClick)
    {
        Countdown(ref patienceLeft, out bool bonus, out bool penalty, minValueBeforeStop: -10);

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
                        MoveCustomer(bedObject.name);
                    }

                    if (Vector3.Distance(transform.position, bedObject.transform.position) <= 1.5f)
                    {
                        AccountPayment(bonus, penalty, bedRent);
                        patienceLeft = -11f; //restart patienceLeft
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
                    AccountPayment(mustWakeUp, false, alarmService);
                    patienceLeft = -0.1f; //restart patienceLeft
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
            Vector3 trWorldPos = transform.position;
            Vector3Int trTilePos = customerTileMap.WorldToCell(trWorldPos);

            if (!transform.parent.name.StartsWith("PAYING_LINE"))
            {
                MoveCustomer(ChoosePayingPoint());
            }

            if (transform.parent.name.StartsWith("PAYING_LINE"))
            {
                if(Vector3.Distance(trTilePos, FindCustomerDest(transform.parent.name)) < 0.2f)
                {
                    Countdown(ref patienceLeft, out bool bonus, out bool penalty, minValueBeforeStop: -10);

                    if (transform.parent.name == "PAYING_LINE_1")
                    {   
                        readyState++;
                        return true;
                    }
                }
            }
        }        
        return false;
    }    

    private bool AtWaitCashier(PlayerLocation playerInstance, PlayerDestination playerSetDestination, GameObject lastClick)
    {
        Countdown(ref patienceLeft, out bool bonus, out bool penalty, minValueBeforeStop: -10);

        if (playerSetDestination == null)
        {
            if (lastClick == gameObject)
            {
                if (playerInstance.location == "PAYING_LINE")
                {
                    AccountPayment(bonus, penalty, free);
                    patienceLeft = -0.1f; //restart patienceLeft
                    readyState++;
                    return true;
                }                    
            }
        }
        return false;
    }

    private bool AtReceiveSuitcase(PlayerDestination playerSetDestination, GameObject lastClick, GameObject previousClick)
    {
        Countdown(ref patienceLeft, out bool bonus, out bool penalty, minValueBeforeStop: -10);

        if (playerSetDestination == null)
        {
            if (previousClick.name == "SUITCASE_CABINET")
            {
                if (lastClick == gameObject)
                {
                    AccountPayment(bonus, penalty, baggageClaim);
                    patienceLeft = -11f; //restart patienceLeft
                    readyState++;
                    return true;
                }
            }
        }
        return false;
    }

    private bool AtLeaving()
    {
        float totalPayment = payment + tipCounter * 5f;
        GameManager.Instance.AddPaymentToIncome(totalPayment);

        Debug.Log("Do animation.");
        return true;
    }

    #region METHODS

    private void ReadyToChangeState()
    {
        stateIndex = readyState;
        GameManager.Instance.AddClickDivider();
    }

    private void SetCustomerTimers()
    {
        if (type == customerType.PARENT) { sleepNeeded = 20f; patienceLeft = 15f; }
        else if (type == customerType.SALARY_MAN) { sleepNeeded = 15f; patienceLeft = 8f; }
        else { sleepNeeded = 10f; patienceLeft = 25f; }
    }

    private void CheckForLeave()
    {
        if (patienceLeft < 0 && Mathf.Clamp(Mathf.Abs(patienceLeft), 0, waitingPatience) == waitingPatience)
        {
            RemoveCustomer();
        }
    }

    private void RunWaitingSequence()
    {
        if (!patienceBar)
        {
            InstantiatePatienceBar();
            waitingPatience = patienceLeft;
        }
        SetPatienceBar(patienceLeft);
        Countdown(ref patienceLeft);
    }

    private void InstantiatePatienceBar()
    {
        //INSTANTIATE PARENT TRANSFORM
        patienceObject = Instantiate(patiencePF, UICanvas.transform);
        patienceObject.name = patiencePF.name;
        patienceBar = patienceObject.GetComponent<PatienceCountdownUI>();

        SetPosition();

        //INSTANTIATE FILL
        GameObject fill = Instantiate(fillPF, patienceObject.transform);
        fill.name = fillPF.name;
    }

    private void SetPosition()
    {
        //PLACE PARENT TO PROPER PLACE ABOVE CUSTOMER
        float rectTransPos_x = transform.position.x * 10f;
        float rectTransPos_y = transform.position.y * 10f + 5f;
        patienceObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(rectTransPos_x, rectTransPos_y, 0f);
    }

    private void Countdown(ref float countdownVariable, out bool bonus, out bool penalty, float maxValue = 10, float minValueBeforeStop = -30f, float countdownRate = 1)
    {
        if (countdownVariable < minValueBeforeStop) { countdownVariable = maxValue; }
        maxValue = countdownVariable;
        //countdownVariable -= Time.deltaTime * countdownRate;
        countdownVariable = Mathf.Clamp(countdownVariable - Time.deltaTime * countdownRate, minValueBeforeStop, maxValue);
        bonus = countdownVariable > 0 ? true : false;
        penalty = countdownVariable == minValueBeforeStop ? true : false;
    }

    private void Countdown(ref float countdownVariable, float maxValue = 10, float minValueBeforeStop = -30f, float countdownRate = 1)
    {
        Countdown(ref countdownVariable, out _, out _,maxValue, minValueBeforeStop, countdownRate);
    }    

    private void SetPatienceBar(float patienceLeft)
    {
        patienceBar.SetStartingPatience(patienceLeft);
        patienceBar.SetPatience(patienceLeft);
        SetPosition();
    }

    private void RemoveCustomer()
    {
        Destroy(this.gameObject);
        Destroy(patienceObject);
    }

    private void DestroyPatienceBar()
    {
        Destroy(patienceObject);
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

    public Vector3Int FindCustomerDest(string destinationName)
    {
        Vector3Int destinationVector = GameManager.Instance.customerPosition[destinationName];

        return destinationVector;
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
        violationCounter = Mathf.Abs(Mathf.Ceil(sleepNeeded / 4f));
        mustWakeUp = (violationCounter == 0) ? true : false;       

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
        GameObject go = Instantiate(dirtySheetsPF, bedObject.transform);
        go.name = dirtySheetsPF.name;
        
    }

    private void StopMeritDemerit()
    {
        countdownOn = false;         
        mustWakeUp = false;

        payment -= violationCounter;
    }

    private string ChoosePayingPoint()
    {
        List<Transform> payingList = PayingCustomerManager.Instance.payingList;
        foreach (Transform tr in payingList)
        {
            if (tr.childCount == 0)
            {
                transform.SetParent(tr);
                return tr.name;
            }
        }
        return bedObject.name;
    }

    public void AccountPayment(bool bonusAvailable, bool penaltyAvailable, float paymentToAdd)
    {
        //CHECK FOR TIPS
        if (bonusAvailable)
        {
            tipCounter++;
            //Debug.Log("Play +1 animation above player head");
        }
        else if (penaltyAvailable)
        {
            tipCounter--;
        }

        //ADD PAYMENTS FOR SERVICES
        payment += paymentToAdd;
    }

    #endregion
}
