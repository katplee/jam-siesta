using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingCustomerManager : Singleton<WaitingCustomerManager>
{
    public static event Action OnCustomerUpdate;

}
