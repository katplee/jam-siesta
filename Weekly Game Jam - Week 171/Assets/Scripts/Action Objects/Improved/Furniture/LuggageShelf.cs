using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuggageShelf : ItemClickable
{
    private Luggage content = null;

    protected override void OnInteractionWithItem(MNode playerPosition)
    {
        if(GetPositionInTileMap() == playerPosition.GetPositionInTileMap())
        {
            Interact();
        }
    }

    //deposit all luggages being held
    private void Interact()
    {
        Player giver = Player.Instance;
        giver.DropItemTo(this, content);
    }
}
