using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation : Singleton<PlayerLocation>
{
    public enum playerLocation
    {
        PLAYER_DEFAULT,
        WAITING_LINE,
        SUITCASE_CABINET,
        PAJAMAS_CABINET,
        TOUR_CHECKPOINT,
        BEDSIDE,
        PAYING_LINE        
    }

    public playerLocation location;
}
