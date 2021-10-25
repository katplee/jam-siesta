using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dresser : ItemClickable
{
    private Pod pod = null;
    private Bed bed = null;
    private DresserAlarm alarm = null;

    private new void Awake()
    {
        pod = GetComponentInParent<Pod>();
        bed = pod.GetComponentInChildren<Bed>();
        alarm = GetComponent<DresserAlarm>();

        base.Awake();
    }

    //turn off the alarm
    //if a player is sleeping in the pod, automatically wake player up
    protected override void Interact()
    {
        //make sure that a player is sleeping in the bed of the same pod
        Customer customer = bed.GetComponentInChildren<Customer>();

        if (!customer) { return; }
        
        UpdateCustomerSatisfaction(customer);

        if (!customer.GetComponent<Sleeping>()) { return; }

        //transport player to corresponding bed node to wake player up
        bed.OnClick();
    }

    public void UpdateCustomerSatisfaction(Customer customer)
    {
        //customer is in the bed's item node at this point
        MNode node = GameManager.Instance.SearchEquivalentNode(customer.GetPositionInTilemap(), customer.label);
        float ticketValue = node.GetTicketValue();

        //it is assumed that when this function is called that the customer's patience meter is on and active
        float grade = alarm.ResetAlarm();
        customer.satisfaction.ComputeSatisfaction(grade, ticketValue);
    }

    /*
    [SerializeField]
    private bool electricitySwitch = true;
    [SerializeField]
    private float timeUntilOff; //{ get; set; }
    [SerializeField]
    private bool canSetAlarm; //{ get; set; }    
    [SerializeField]
    private bool mustTurnOff; //{ get; set; }
    [SerializeField]
    private bool alarm = false;
    [SerializeField]
    private GameObject child = null;

    private GameObject alarmObject;
    private GameObject alarmPF;
    private GameObject fillPF;
    private GameObject UICanvas;
    private AlarmCountdownUI alarmBar = null;

    private void Start()
    {
        UICanvas = Containers.Instance.UICanvas;
        alarmPF = Prefabs.Instance.alarmPF;
        fillPF = Prefabs.Instance.fillPF;
    }

    private void Update()
    {
        if (Utilities.HasSiblingWithAChildWithComponent<Customer>(gameObject, out child))
        {
            timeUntilOff = Mathf.Clamp(child.GetComponent<Customer>().sleepNeeded, 0f, 20f);
            SetAlarmValues();            

            if (child.GetComponent<Customer>().countdownOn)
            {
                if (electricitySwitch)
                {
                    canSetAlarm = (!alarm) ? true : false;
                    mustTurnOff = (timeUntilOff == 0f && alarm) ? true : false;
                }
            }
        }
        else
        {
            if (alarmObject) { Destroy(alarmObject); }
            electricitySwitch = true;
            timeUntilOff = 0f;
            canSetAlarm = false;
            mustTurnOff = false;
            alarm = false;
        }
    }

    private void LateUpdate()
    {
        if (electricitySwitch)
        {
            if (Vector3.Distance(PlayerLocation.Instance.transform.position, transform.position) <= 1.5)
            {
                if (canSetAlarm)
                {
                    alarm = true;
                    canSetAlarm = false;
                }

                if (mustTurnOff)
                {
                    alarm = false;
                    mustTurnOff = false;
                    electricitySwitch = false;
                    Debug.Log("Stop alarm animation!");
                }

                if (alarm)
                {
                    Debug.Log("Display remaining time with time bar using timeUntilOff variable.");
                }
            }
        }
    }

    private void SetAlarmValues()
    {
        if (!alarmBar)
        {
            InstantiateAlarmBar();
        }
        SetAlarmBarVisibility();
        SetAlarmBar(timeUntilOff);
    }

    private void InstantiateAlarmBar()
    {
        //INSTANTIATE ALARM
        alarmObject = Instantiate(alarmPF, UICanvas.transform);
        alarmObject.name = gameObject.name.Replace("DRESSER", "ALARM");
        alarmBar = alarmObject.GetComponent<AlarmCountdownUI>();
        
        //SET POSITION ABOVE DRESSER        
        float rectTransPos_x = transform.position.x * 10f;
        float rectTransPos_y = transform.position.y * 10f + 7f;

        alarmObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(rectTransPos_x, rectTransPos_y, 0f);
        
        //INSTANTIATE FILL
        GameObject fill = Instantiate(fillPF, alarmObject.transform);
        fill.name = gameObject.name.Replace("DRESSER", "FILL");
    }

    private void SetAlarmBarVisibility()
    {
        if (!alarm)
        {
            if (alarmBar) { alarmBar.HideBar(); }
        }
        else
        {
            alarmBar.ShowBar();
        }
    }

    private void SetAlarmBar(float timeUntilOff)
    {
        alarmBar.SetStartingAlarm(timeUntilOff);
        alarmBar.SetAlarm(timeUntilOff);
        if(timeUntilOff == 0) { alarmBar.ResetAlarm(); }
    }

    /*
    private bool SetAlarmOperation(out bool oversleep)
    {        
        timeUntilOff = child.GetComponent<Customer>().sleepNeeded;
        oversleep = (Mathf.Sign(timeUntilOff) == -1) ? true : false;

        return child.GetComponent<Customer>().countdownOn;
    }
    */
}
