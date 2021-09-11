using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DresserAlarm : MonoBehaviour
{
    //patience-related user interface parameters
    private UIAlarm customerAlarm = null;

    //patience amount parameters
    private float alarm = 0f;
    private float _alarm = 0f; //current alarm value
    private float allowance = 0f;

    private void Start()
    {
        //hide the alarm bar initially
        ToggleVisibility();
    }

    public void SetAlarmParameters(float alarm, float allowance)
    {
        this.alarm = alarm;
        _alarm = this.alarm;
        this.allowance = allowance;
    }

    public bool UpdateAlarm()
    {
        _alarm = Mathf.Max(_alarm, -alarm);
        if (_alarm == -alarm) { return false; }

        if (Mathf.Max(_alarm, 0) == 0 && allowance != 0)
        {
            allowance = Mathf.Max(allowance - Time.deltaTime, 0);
            customerAlarm.ChangeFillAmount(1f, 1f);
            customerAlarm.ChangeFillColor(true);
        }
        else
        {
            _alarm -= Time.deltaTime;
            customerAlarm.ChangeFillAmount(Mathf.Abs(_alarm), alarm);
            customerAlarm.ChangeFillColor(false);
        }

        return true;
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

    private void SetAlarm(UIAlarm alarm)
    {
        customerAlarm = alarm;
    }

    public void ToggleVisibility()
    {
        bool toggle = (customerAlarm.gameObject.activeSelf) ? false : true;
        customerAlarm.gameObject.SetActive(toggle);
    }
}
