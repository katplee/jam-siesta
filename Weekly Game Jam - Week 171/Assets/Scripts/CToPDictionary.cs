using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CToPDictionary : Singleton<CToPDictionary>
{
    public Dictionary<Customer.customerState, string> cToPPosition =
        new Dictionary<Customer.customerState, string>()
        {
            { Customer.customerState.WAIT_TOUR, "PLAYER_TOUR_CHECKPOINT" },
            { Customer.customerState.YES_BED, "PLAYER_TOUR_CHECKPOINT" },
        };

}
