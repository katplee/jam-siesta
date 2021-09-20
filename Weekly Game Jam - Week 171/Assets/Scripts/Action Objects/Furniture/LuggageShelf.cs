using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuggageShelf : ItemClickable
{
    private new Luggage content = null;
    private bool toggle = false;

    public override void OnClick()
    {
        //check if player is about to serve a paying customer
        bool terminate = CheckPaying();

        //transport player to corresponding player node
        if (!terminate) { base.OnClick(); }
    }

    private bool CheckPaying()
    {
        bool terminate = false;
        //make sure that the player clicked an object with AITING tag before this object
        if (Player.Instance.GetActiveTag(out Customer customer) as Paying)
        {
            toggle = true;
        }
        return terminate;
    }

    //deposit all luggages being held
    protected override void Interact()
    {
        if (toggle)
        {
            Player receiver = Player.Instance;
            receiver.GetItemFrom(this, content, out List<ItemTransferrable> items);
        }
        else
        {
            Player giver = Player.Instance;
            giver.DropItemTo(this, content);
        }
    }
}
