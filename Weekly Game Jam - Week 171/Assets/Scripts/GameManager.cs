using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    public List<GameObject> clickRecord;
    public Transform clickDividerTransform;
    public GameObject lastClickedCustomer = null;
    public GameObject clickedObject = null; //change this to private
    public string clickedObjectName; //change this to private
    [SerializeField]
    private LayerMask clickableObjectsLayer;

    [SerializeField]
    private Camera camera;
    public Transform playerTransform;

    //public GameObject trial;    

    public Dictionary<string, Vector3Int> playerPosition = new Dictionary<string, Vector3Int>();
    public Dictionary<string, Vector3Int> customerPosition = new Dictionary<string, Vector3Int>();
    private Dictionary<Customer.customerState, string> playerPosDict;

    /*
    public enum objectType
    {
        NONE,
        CUSTOMER,
        SUITCASE_CABINET,
        PAJAMAS_CABINET,
        BED,
        DRESSER,
        CLEAN_SHEETS_CABINET,
        LAUNDRY
    }
    */

    private void Start()
    {
        AddClickDivider();
        AddClickDivider();
        playerPosDict = GetComponent<CToPDictionary>().cToPPosition;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(playerTransform.GetComponent<PlayerDestination>() == null)
            {
                RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector3.zero, Mathf.Infinity, clickableObjectsLayer);

                if(hit.collider != null)
                {
                    SetClickedObject(hit.collider);
                    playerTransform.gameObject.AddComponent<PlayerDestination>();
                    PlayerDestination.Instance.Destination = DecidePlayerDestination();
                }
            }
        }

        MoreUpdate(clickRecord, ref lastClickedCustomer);
    }

    private Vector3Int DecidePlayerDestination()
    {
        //IF WITHOUT DIRTY SHEETS, YOU CAN SELECT ANYTHING
        if (playerTransform.childCount == 0)
        {
            switch (clickedObjectName)
            {
                case "CUSTOMER":
                    return CustomerToPlayer();

                case "SUITCASE_CABINET":
                case "PAJAMAS_CABINET":
                case "CLEAN_SHEETS_CABINET":
                case "LAUNDRY":
                    return playerPosition[clickedObjectName];                
            }

            if (clickedObjectName.StartsWith("DRESSER"))
            {
                return playerPosition[clickedObjectName];
            }

            if (clickedObjectName.StartsWith("BED"))
            {
                return BedChecklist(clickedObjectName);
            }
        }
        //IF WITH DIRTY SHEETS, ONLY CLICKABLE AREA IS WASHING MACHINE
        else
        {
            switch (clickedObjectName)
            {
                case "LAUNDRY":
                    return playerPosition[clickedObjectName];
            }
            return playerPosition["PLAYER_LOCATION"];
        }
        return playerPosition["PLAYER_LOCATION"];
    }

    private Vector3Int CustomerToPlayer()
    {
        Customer.customerState customerState = clickedObject.GetComponent<Customer>().State;
        string playerPos = playerPosDict[customerState];
        return playerPosition[playerPos];
    }

    private Vector3Int BedChecklist(string bedName)
    {
        bool go = true;
        bool isAvailable = clickedObject.GetComponent<Bed>().IsAvailable;
        bool isWithCustomer = (PlayerLocation.Instance.location == "PLAYER_TOUR_CHECKPOINT");

        if (isWithCustomer) { go = isAvailable; }

        return go ? playerPosition[bedName] : playerPosition["PLAYER_LOCATION"];
    }    

    private void MoreUpdate(List<GameObject> clickRecord, ref GameObject lastCustomer)
    {
        //***KEEP CLICK LIST AT A MAX OF 8 ELEMENTS
        if(clickRecord.Count > 8)
        {
            clickRecord.Remove(clickRecord.First());
        }

        //***SAVE LAST CUSTOMER GAME OBJECT
        if(clickedObjectName == "CUSTOMER")
        {
            lastCustomer = clickedObject;
        }
    }

    public void AddClickDivider()
    {
        clickRecord.Add(clickDividerTransform.gameObject);
    }    

    public void SetClickedObject(Collider2D collider)
    {
        clickRecord.Add(collider.gameObject);
        this.clickedObject = collider.gameObject;
        this.clickedObjectName = clickedObject.name;        
    }    
}
