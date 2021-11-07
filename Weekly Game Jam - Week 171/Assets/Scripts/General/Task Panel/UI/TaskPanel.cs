using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskPanel : MonoBehaviour
{
    private static TaskPanel instance;
    public static TaskPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TaskPanel>();
            }
            return instance;
        }
    }

    private List<GameObject> destinationQueue = new List<GameObject>();
    private Transform slotTemplate = null;
    private Transform container = null;

    public void DeclareThis<T>(string element, T UIObject)
        where T : UIObject
    {
        switch (element)
        {
            case "Container":
                SetContainer(UIObject as Container);
                break;

            case "SlotTemplate":
                SetSlotTemplate(UIObject as SlotTemplate);
                break;

            default:
                break;
        }
    }

    private void SetContainer(Container container)
    {
        this.container = container.transform;
    }

    private void SetSlotTemplate(SlotTemplate template)
    {
        this.slotTemplate = template.transform;
    }

    public void GenerateInstance(DestinationScriptable destination)
    {
        /*
        if (destinationQueue.Count != 0)
        {
            if (destinationQueue.ElementAt(destinationQueue.Count - 1).name == destination.itemName.GetType().Name)
            {
                //no consecutive clicking of the same 
                return;
            }
        }
        */

        //generate the icon that would appear in the UI
        Transform transform = Instantiate(slotTemplate, container);
        //destroy the slot template script
        Destroy(transform.GetComponent<SlotTemplate>());
        //change the game object's name for better understandability
        transform.name = destination.itemName.GetType().Name;
        //add the item to the destination queue
        destinationQueue.Add(transform.gameObject);
    }

    public void ClearItem(GameObject item = null)
    {
        //the default assumption is that the first destination in the queue will be cleared
        int index = 0;

        if (item)
        {
            //if there is a specific item to clear from the queue, find the index first
            index = IndexOf(item, destinationQueue);
            Debug.Log(index);
        }

        GameObject destination = destinationQueue[index];
        
        //delete the destination from the queue
        //destroy the icon in the task panel
        if (destination.name == container.GetChild(index).name)
        {
            Destroy(container.GetChild(index));
        }
        else
        {
            Debug.Log("The panel and the queue is not synced.");
        }
        
        PlayerController.Instance.DequeuePath()
    }

    private int IndexOf(GameObject item, List<GameObject> list)
    {
        int i = 0;

        foreach (GameObject obj in list)
        {
            if (obj == item) { return i; }
            i++;
        }

        return -1;
    }
}
