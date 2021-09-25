﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : ItemClickable
{
    private Pajamas _content = null;
    private PlayerController playerController = null;

    private new void Awake()
    {
        //set the content
        content = new Sheets();
        playerController = PlayerController.Instance;

        //do the usual parameter-setting
        base.Awake();
    }

    public override void OnClick()
    {
        //check if player is about to serve a waiting customer
        bool terminate = CheckWaiting();

        //do not approach the bed if player is currently in motion or if the bed is still dirty
        //and therefore, cannot be assigned to a new customer
        if (playerController.IsMoving() || terminate) { Player.Instance.RestartTags(); return; }

        //transport player to corresponding player node
        if (!terminate) { base.OnClick(); }
    }

    private bool CheckWaiting()
    {
        bool terminate = false;
        //make sure that the player clicked an object with WAITING tag before this object
        if (Player.Instance.GetActiveTag(out Customer customer) as Waiting)
        {
            foreach (Transform item in transform)
            {
                //terminate operation if bed is dirty with sheets and pajamas
                if (item.GetComponent<Sheets>()) { terminate = true; break; }
                if (item.GetComponent<Pajamas>()) { terminate = true; break; }

                //terminate operation if pod is occupied by another customer
                if (GetComponentInChildren<Customer>()) { terminate = true; break; }
            }

            //transport customer to corresponding customer node
            if (!terminate) 
            {
                customer.UpdateCustomerSatisfaction();
                customer.controller.TransportCustomer(customerNode); 
            }
        }
        return terminate;
    }

    protected override void Interact()
    {
        //wake a sleeping customer up
        if (GetComponentInChildren<Sleeping>())
        {
            Sleeping tag = GetComponentInChildren<Sleeping>();
            CustomerController controller = tag.gameObject.GetComponent<CustomerController>();
            controller.TransportCustomer(customerNode);
        }

        //clean bed, retrieve sheets and pajamas
        if (GetComponentInChildren<ItemTransferrable>())
        {
            Player receiver = Player.Instance;

            //retrieve the sheets and change it to a dirty item transferrable
            bool received = receiver.GetItemFrom(this, -1, content as Sheets, out List<ItemTransferrable> sheets);
            if (received) { AddDirtyTag(sheets); }

            //retrieve the pajamas and change it to a dirty item transferrable
            received = receiver.GetItemFrom(this, -1, _content, out List<ItemTransferrable> pajamas);
            if (received) { AddDirtyTag(pajamas); }
        }
    }

    public ItemTransferrable LeaveSheets()
    {
        ItemTransferrable sheets = GenerateContent();
        return sheets;
    }

    private void AddDirtyTag(List<ItemTransferrable> clean)
    {
        foreach (ItemTransferrable item in clean)
        {
            item.gameObject.AddComponent<Dirty>();
        }
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
