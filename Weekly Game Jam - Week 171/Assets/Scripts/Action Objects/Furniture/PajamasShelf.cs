using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PajamasShelf : ItemClickable
{
    private Pajamas content = new Pajamas();
    private GameObject contentObject = new GameObject();

    private ItemTransferrable GenerateContent()
    {
        GameObject go = Instantiate(contentObject, container);
        Pajamas component = go.AddComponent<Pajamas>();
        return component;
    }

    protected override void Interact()
    {
        Debug.Log("hello");
        ItemTransferrable[] item = new ItemTransferrable[] { GenerateContent() };

        Player receiver = Player.Instance;
        receiver.ReceiveItem(item);
    }
}
