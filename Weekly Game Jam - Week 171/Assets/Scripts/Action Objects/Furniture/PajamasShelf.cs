using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PajamasShelf : ItemClickable
{
    private new void Awake()
    {
        //set the content
        content = new Pajamas();

        //do the usual parameter-setting
        base.Awake();
    }

    //give player a set of pajamas
    protected override void Interact()
    {
        ItemTransferrable[] item = new ItemTransferrable[] { GenerateContent() };

        Player receiver = Player.Instance;
        receiver.ReceiveItem(item);
    }
}
