using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPatience : MonoBehaviour
{
    //patience-related user interface parameters
    private UIPatience customerPatience = null;

    //patience amount parameters
    private float patience = 5f; //patience of customer in seconds
    private float _patience = 0f; //current patience value

    private void Awake()
    {
        _patience = patience;
    }

    private void Update()
    {
        //UpdatePatience();
    }

    private void SetPatience(UIPatience patience)
    {
        customerPatience = patience;
    }

    private bool UpdatePatience()
    {
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
        Canvas patience = GetComponentInChildren<Canvas>();
        if (patience.GetComponentInChildren<UIPatience>())
        {
            patience.enabled = status;
        }
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
}
