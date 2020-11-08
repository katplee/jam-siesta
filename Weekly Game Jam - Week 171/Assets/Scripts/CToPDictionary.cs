using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CToPDictionary : Singleton<CToPDictionary>
{
    public Dictionary<Customer.customerState, string> cToPPosition =
        new Dictionary<Customer.customerState, string>()
        {
            { Customer.customerState.GIVE_SUITCASE, "WAITING_LINE" },
            { Customer.customerState.WAIT_PAJAMAS, "WAITING_LINE" },
            { Customer.customerState.WAIT_TOUR, "PLAYER_TOUR_CHECKPOINT" },
            { Customer.customerState.YES_BED, "PLAYER_TOUR_CHECKPOINT" },
            { Customer.customerState.WAIT_CASHIER, "PAYING_LINE" },
            { Customer.customerState.RECEIVE_SUITCASE, "PAYING_LINE" },
        };

}
