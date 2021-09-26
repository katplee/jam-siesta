using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerformance : MonoBehaviour
{
    private float money = 0f;
    private float indexSum = 0f;
    private int customerCount = 0;
    private float performance = 5f;

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
}
