using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSatisfaction : MonoBehaviour
{
    private Customer customer = null;
    private float satisfactionIndex = 1;

    //prices for customers
    //all customers need to pay 200 sies for the basic bunk bed
    private float basic = 100;
    //add prices for a private room, for special beds, etc.

    private void Awake()
    {
        customer = GetComponent<Customer>();
    }

    public void ComputeSatisfaction(float grade, float ticketValue)
    {
        float sign = Mathf.Sign(grade);
        //demerit and merit functions might not be needed after all, since sign is multiplied to ticket value
        satisfactionIndex += sign * ticketValue;
    }

    public void Demerit(float demerit)
    {
        satisfactionIndex += demerit;
    }

    public void Merit(float merit)
    {
        satisfactionIndex += merit;
    }

    public float ComputePayment()
    {
        return basic * satisfactionIndex;
    }
}
