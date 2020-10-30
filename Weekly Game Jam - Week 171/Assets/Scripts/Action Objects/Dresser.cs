using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dresser : MonoBehaviour
{
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

    private void Update()
    {
        if(Utilities.HasSiblingWithAChildWithComponent<Customer>(gameObject, out child))
        {
            timeUntilOff = Mathf.Clamp(child.GetComponent<Customer>().sleepNeeded, 0f, 20f);

            if (child.GetComponent<Customer>().countdownOn)
            {
                if (electricitySwitch)
                {
                    if (!alarm)
                    {
                        canSetAlarm = true;
                    }

                    if (timeUntilOff == 0f)
                    {
                        if (alarm)
                        {
                            mustTurnOff = true;
                        }

                    }
                }                
            }
        }
        else
        {
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
            if (Vector3.Distance(PlayerLocation.Instance.gameObject.transform.position, transform.position) <= 1.5)
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

    private bool SetAlarmOperation(out bool oversleep)
    {        
        timeUntilOff = child.GetComponent<Customer>().sleepNeeded;
        oversleep = (Mathf.Sign(timeUntilOff) == -1) ? true : false;

        return child.GetComponent<Customer>().countdownOn;
    }
}