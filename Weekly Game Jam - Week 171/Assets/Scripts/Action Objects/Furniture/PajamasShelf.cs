using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PajamasShelf : ItemClickable
{
    private Pajamas content = new Pajamas();
    private GameObject contentObject;

    private ItemTransferrable GenerateContent()
    {
        contentObject = new GameObject();
        GameObject go = contentObject;
        go.transform.SetParent(container);
        go.name = content.GetType().Name;
        Pajamas component = go.AddComponent<Pajamas>();
        return component;
    }

    //give player a set of pajamas
    protected override void Interact()
    {
        ItemTransferrable[] item = new ItemTransferrable[] { GenerateContent() };

        Player receiver = Player.Instance;
        receiver.ReceiveItem(item);
    }
}
