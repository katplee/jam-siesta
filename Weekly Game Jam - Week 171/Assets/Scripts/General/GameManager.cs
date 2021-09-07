using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    public static event Action OnMouseClick;

    private Dictionary<Vector3Int, CustomerNode> customerNodes =
        new Dictionary<Vector3Int, CustomerNode>();
    private Dictionary<Vector3Int, PlayerNode> playerNodes =
        new Dictionary<Vector3Int, PlayerNode>();

    public static void InvokeMouseResponse()
    {
        //Debug.Log("hello");
        OnMouseClick?.Invoke();
    }

    public MNode RefreshNodeParent<T>(T element)
        where T : Element
    {
        Vector3Int currentPosition = element.GetPositionInTilemap();

        if (element as Player)
        {
            if (playerNodes.ContainsKey(currentPosition))
            {
                MNode newParent = playerNodes[currentPosition].ParentObject(element.gameObject);
                return newParent;
            }
        }
        else
        {
            if (customerNodes.ContainsKey(currentPosition))
            {
                MNode newParent = customerNodes[currentPosition].ParentObject(element.gameObject);
                return newParent;
            }
        }
        return null;
    }

    public void AddNode<T>(string type, Vector3Int position, T node)
        where T : MNode
    {
        switch (type)
        {
            case "CustomerNode":
                customerNodes.Add(position, node as CustomerNode);
                break;

            case "PlayerNode":
                playerNodes.Add(position, node as PlayerNode);
                break;

            default:
                break;
        }
    }

    public MNode SearchClosestNode(Vector3Int source, string closestNodeType)
    {
        switch (closestNodeType)
        {
            case "Customer":
                return CompareThisLocation(source, playerNodes);

            default:
                break;
        }

        return null;
    }

    private MNode CompareThisLocation<T>(Vector3Int location, Dictionary<Vector3Int, T> nodes)
        where T : MNode
    {
        MNode closestNode = null;
        float shortestDistance = -1f;

        foreach (KeyValuePair<Vector3Int, T> node in nodes)
        {
            float _distance = Vector3Int.Distance(location, node.Key);

            if (shortestDistance < 0)
            {
                shortestDistance = _distance;
                closestNode = node.Value;
            }
            else if (_distance > shortestDistance)
            {
                continue;
            }
            else if (_distance == 0)
            {
                continue;
            }
            else if(_distance < shortestDistance)
            {
                shortestDistance = _distance;
                closestNode = node.Value;
            }
        }
        return closestNode;
    }



    /*
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

    [SerializeField]
    private float income;

    public Dictionary<string, Vector3Int> playerPosition = new Dictionary<string, Vector3Int>();
    public Dictionary<string, Vector3Int> customerPosition = new Dictionary<string, Vector3Int>();
    private Dictionary<Customer.customerState, string> playerPosDict;

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
                    return ConvertCustomerToPlayer();

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
        //IF WITH DIRTY SHEETS, ONLY THOSE INCLUDED BELOW CAN BE CLICKED
        else
        {
            switch (clickedObjectName)
            {
                case "LAUNDRY":
                case "CLEAN_SHEETS_CABINET":
                    return playerPosition[clickedObjectName];
            }

            if (clickedObjectName.StartsWith("BED"))
            {
                return BedChecklist(clickedObjectName);
            }

            return playerPosition["PLAYER_LOCATION"];
        }
        return playerPosition["PLAYER_LOCATION"];
    }

    private Vector3Int ConvertCustomerToPlayer()
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
        //REMOVE DESTROYED OBJECTS IN THE LIST
        foreach(GameObject go in clickRecord)
        {
            if(go == null) { clickRecord.Remove(go); }
        }

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

        //SET LAST CLICKED CUSTOMER TO NULL IF MISSING
        if(!lastClickedCustomer) { lastClickedCustomer = null; }
        if(!clickedObject) { clickedObject = null; }

        //InvalidOperationException: Collection was modified; enumeration operation may not execute.
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

    public void AddPaymentToIncome(float payment)
    {
        income += payment;
    }
    */
}
