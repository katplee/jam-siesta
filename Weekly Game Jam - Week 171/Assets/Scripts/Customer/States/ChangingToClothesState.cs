using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingToClothesState : StateMachineBehaviour
{
    /*
     * checkPoint : the location at which the customer must go to before 
     * dropoffPoint : the location at which the player must go to before receiving an item/before a state change
     * dropoffItem : the item which will be given/received by the receiver
     * receiver : the receipient of the item to be transferred
     */

    private Customer customer = null;
    private CustomerController controller = null;
    private SpriteRenderer renderer = null;
    private Animator animator = null;
    private Pod pod = null;
    private Bed bed = null;
    private float blinkingTime = 0f;
    private bool end = false;

    //parameters related to completion of task
    //private Vector3Int checkPoint = new Vector3Int(-12, 5, 0);
    //private Vector3Int dropoffPoint = new Vector3Int(-8, 5, 0);
    private Pajamas dropoffItem = new Pajamas();
    //private Player receiver = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        customer = animator.gameObject.GetComponent<Customer>();
        controller = customer.GetComponent<CustomerController>();
        renderer = customer.GetComponent<SpriteRenderer>();
        this.animator = animator;
        pod = customer.GetComponentInParent<Pod>();
        bed = pod.GetComponentInChildren<Bed>();
        blinkingTime = customer.GetChangingTime();

        SubscribeEvents();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (end) { return; }

        AnimateElement();
    }

    private void CheckForEndState(MNode node)
    {
        if (AnimateElement())
        {
            bool end = TransferItem();
            if (end) 
            {
                UnsubscribeEvents();
                animator.SetTrigger("MoveState"); 
            }
        }
    }

    private bool AnimateElement()
    {
        if (!end)
        {
            blinkingTime -= Time.deltaTime;
            bool blink = (renderer.enabled) ? false : true;
            renderer.enabled = blink;

            if (Mathf.Max(blinkingTime, 0) != 0) { return false; }

            renderer.enabled = true;

            end = true;

            controller.InvokeMoveCompleteEvent();
        }

        return true;
    }

    private bool TransferItem()
    {
        Customer giver = customer;
        bool transfered = giver.DropItemTo(bed, dropoffItem);
        bed.LeaveDirtySheets();

        return transfered;
    }
    
    //private void PerformStateProcesses() { }

    private void SubscribeEvents()
    {
        controller.OnMoveComplete += CheckForEndState;
    }

    private void UnsubscribeEvents()
    {
        controller.OnMoveComplete -= CheckForEndState;
    }
}
