using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerPatience : MonoBehaviour
{
    //patience-related user interface parameters
    private UIPatience customerPatience = null;
    private Canvas canvas = null;

    //patience amount parameters
    private int patience = 2; //patience of customer in seconds
    private float _patience = 0f; //current patience value
    private bool end = false;

    public void SetParameters(int sleepIndex)
    {
        int patienceIndex = 0;
        switch (sleepIndex)
        {
            //salaryman
            case 1: patienceIndex = 1; break;
            //parent
            case 2: patienceIndex = 3; break;
            //student
            case 3: patienceIndex = 2; break;
            //random
            case 4: patienceIndex = Random.Range(1, 5); break;

            default: break;
        }

        patience *= patienceIndex;
        _patience = patience;
    }

    private void SetPatience(UIPatience patience)
    {
        customerPatience = patience;
    }

    public bool UpdatePatience()
    {
        if (end) { return false; }

        _patience = Mathf.Max(_patience, -patience);
        if (_patience == -patience) { return false; }

        _patience -= Time.deltaTime;
        bool plus = (Mathf.Sign(_patience) == 1) ? true : false;

        customerPatience.ChangeFillAmount(Mathf.Abs(_patience), patience);
        customerPatience.ChangeFillColor(plus);
        return true;
    }

    public void SetPatienceInteractibility(bool status)
    {
        if (!canvas) { canvas = GetComponentInChildren<Canvas>(); }
        canvas.enabled = status;
        end = status ? false : true;
    }

    public void DeclareThis<T>(string type, T UIObject)
        where T : IUserInterface
    {
        switch (type)
        {
            case "UIPatience":
                SetPatience(UIObject as UIPatience);
                break;

            default:
                break;
        }
    }

    public float ResetPatience()
    {
        SetPatienceInteractibility(false);
        float grade = _patience / patience;
        _patience = patience;
        return grade;
    }
}
