using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DresserAlarm : MonoBehaviour
{
    //patience-related user interface parameters
    private UIAlarm customerAlarm = null;
    private Canvas canvas = null;

    //patience amount parameters
    private float alarm = 0f;
    private float _alarm = 0f; //current alarm value
    private float allowance = 0f;

    private void Start()
    {
        //hide the alarm bar initially
        ToggleVisibility();
    }

    public void SetParameters(float alarm, float allowance)
    {
        this.alarm = alarm;
        _alarm = alarm;
        this.allowance = allowance;
    }

    private void SetAlarm(UIAlarm alarm)
    {
        customerAlarm = alarm;
    }

    public bool UpdateAlarm(out float excess)
    {
        _alarm = Mathf.Max(_alarm, -alarm);
        if (_alarm == -alarm) { excess = _alarm; return false; }

        if (Mathf.Max(_alarm, 0) == 0 && allowance != 0)
        {
            //this is the right time to wake up customer
            _alarm = 0; //to avoid problems with the customer index setting
            allowance = Mathf.Max(allowance - Time.deltaTime, 0);
            customerAlarm.ChangeFillAmount(1f, 1f);
            customerAlarm.ChangeFillColor(true);
        }
        else
        {
            //alarm counts down or counts up
            _alarm -= Time.deltaTime;
            customerAlarm.ChangeFillAmount(Mathf.Abs(_alarm), alarm);
            customerAlarm.ChangeFillColor(false);
        }

        excess = _alarm;
        return true;
    }

    public void ToggleVisibility(string preference = "")
    {
        if (!canvas) { canvas = GetComponentInChildren<Canvas>(); }

        switch (preference)
        {
            case "true":
                canvas.enabled = true;
                break;
            case "false":
                canvas.enabled = false;
                break;
            default:
                bool toggle = (canvas.enabled) ? false : true;
                canvas.enabled = toggle;
                break;
        }
    }

    public void DeclareThis<T>(string type, T UIObject)
        where T : IUserInterface
    {
        switch (type)
        {
            case "UIAlarm":
                SetAlarm(UIObject as UIAlarm);
                break;

            default:
                break;
        }
    }

    public float ResetAlarm()
    {
        ToggleVisibility("false");
        float grade = _alarm / alarm;
        grade = (grade == 0) ? 1 : -1;

        //reset timers
        _alarm = 0f;
        _alarm = 0f; //current alarm value
        allowance = 0f;

        return grade;
    }
}
