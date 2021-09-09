﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuggageShelf : ItemClickable
{
    //deposit all luggages being held
    protected override void Interact()
    {
        Player giver = Player.Instance;
        giver.DropItemTo<Luggage>(this);
    }
}
