using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerformance : MonoBehaviour
{
    private static PlayerPerformance instance;
    public static PlayerPerformance Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerPerformance>();
            }
            return instance;
        }
    }

    private float money = 0f;
    private float indexSum = 0f;
    private int customerCount = 0;
    private float performance = 5f;

    //UI-related parameters
    private UIMoneyContainer moneyContainerUI = null;

    public float AddCustomerIndex(float index)
    {
        indexSum += index;
        return indexSum;
    }

    public int IncreaseCustomerCount()
    {
        customerCount++;
        return customerCount;
    }

    public float AddCustomerPayment(float payment)
    {
        money += payment;
        return money;
    }

    public void DeclareThis<T>(string element, T UIobject)
        where T : UIObject
    {
        switch (element)
        {
            case "UIItemContainer":
                moneyContainerUI = UIobject as UIMoneyContainer;
                break;
        }
    }
}
