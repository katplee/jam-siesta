using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuggageShelf : ItemClickable
{
    private new Luggage content = null;
    private bool toggle = false;
    private Customer luggageOwner = null;

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
        //make sure that the player clicked an object with WAITING tag before this object
        if (Player.Instance.GetActiveTag(out Customer customer) as Paying)
        {
            toggle = true;
            luggageOwner = customer;
        }
        return terminate;
    }

    //deposit all luggages being held
    protected override void Interact()
    {
        if (toggle)
        {
            //get player's destination
            PlayerNode destination = luggageOwner.GetComponentInChildren<PlayerNode>();

            //check if player will be issued a ticket for losing luggage
            Player receiver = Player.Instance;
            if (!receiver.GetItemFrom(this, 1, content, out List<ItemTransferrable> items, luggageOwner)) 
            {
                destination.gameObject.AddComponent<LostLuggageTicket>();
                destination.SetTicket();
            }

            Player.Instance.GetComponent<PlayerController>().TransportPlayer(destination.GetPositionInTileMap());

            //restart toggle and customer
            toggle = false;
            luggageOwner = null;
        }
        else
        {
            Player giver = Player.Instance;
            giver.DropItemTo(this, content);
        }
    }
}
