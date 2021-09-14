using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : ItemClickable
{
    private new void Awake()
    {
        //set the content
        content = new Sheets();

        //do the usual parameter-setting
        base.Awake();
    }

    public override void OnClick()
    {
        //make sure that the player clicked an object with WAITING tag before this object
        if (Player.Instance.GetActiveTag(out Customer customer) as Waiting)
        {
            //transport customer to corresponding customer node
            customer.GetComponent<CustomerController>().TransportCustomer(customerNode);
        }

        //transport player to corresponding player node
        base.OnClick();
    }

    protected override void Interact()
    {
        if (GetComponentInChildren<Sleeping>())
        {
            Sleeping tag = GetComponentInChildren<Sleeping>();
            CustomerController controller = tag.gameObject.GetComponent<CustomerController>();
            controller.TransportCustomer(customerNode);
        }

        if (GetComponentInChildren<ItemTransferrable>())
        {
            Player receiver = Player.Instance;
            //receiver.ReceiveItem(item);
        }
    }

    public void LeaveDirtySheets()
    {
        GenerateContent();
    }

    /*
    protected override void Interact()
    {
        Player giver = Player.Instance;
        giver.DropItemTo(this, content);
    }
    */

    /*
    public bool IsAvailable { get; set; }
    public bool IsDirty { get; set; }

    [SerializeField]
    private float timeToClean = 2f;
    public float timeFromClean;

    List<GameObject> clickRecord;

    private void Start()
    {
        clickRecord = GameManager.Instance.clickRecord;
    }

    private void Update()
    {
        IsAvailable = (transform.childCount != 0) ? false : true;

        foreach(Transform tr in transform)
        {
            if (tr.GetComponent<DirtySheets>() != null)
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (IsDirty)
        {
            if (clickRecord[clickRecord.Count - 2] != null && clickRecord[clickRecord.Count - 2].name == "CLEAN_SHEETS_CABINET")
            {
                if (clickRecord[clickRecord.Count - 1]!= null && clickRecord[clickRecord.Count - 1] == gameObject)
                {
                    if (Vector3.Distance(PlayerLocation.Instance.transform.position, transform.position) <= 1.5)
                    {
                        CleanSheets();
                    }
                }
            }
        }
        else
        {
            timeFromClean = 0f;
        }
    }

    private void CleanSheets()
    {
        timeFromClean += Time.deltaTime;

        if (Mathf.Clamp(timeFromClean, 0f, timeToClean) == timeToClean)
        {
            timeFromClean = 0f;

            GameObject dirtySheets;
            if(Utilities.HasChildWithComponent<DirtySheets>(gameObject, out dirtySheets))
            {
                dirtySheets.transform.SetParent(PlayerLocation.Instance.transform);
            }            
        }
    }

    */
}
