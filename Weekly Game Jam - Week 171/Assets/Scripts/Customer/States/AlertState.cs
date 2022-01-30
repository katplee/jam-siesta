using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : StateMachineBehaviour
{
    /*
     * checkPoint : the location at which the customer must go to before 
     * dropoffPoint : the location at which the player must go to before receiving an item/before a state change
     * dropoffItem : the item which will be given/received by the receiver
     * receiver : the receipient of the item to be transferred
     */

    private Customer customer = null;
    private CustomerController controller = null;
    private Animator animator = null;
    private Pod pod = null;
    private Bed bed = null;
    private DresserAlarm alarm = null;
    private bool end = false;

    //parameters related to completion of task
    private CustomerNode checkPoint = new CustomerNode();
    //private Vector3Int dropoffPoint = new Vector3Int(-8, 5, 0);
    //private Luggage dropoffItem = new Luggage();
    //private Player receiver = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        customer = animator.gameObject.GetComponent<Customer>();
        controller = customer.controller;
        this.animator = animator;
        pod = customer.GetComponentInParent<Pod>();
        bed = pod.GetComponentInChildren<Bed>();
        alarm = pod.GetComponentInChildren<DresserAlarm>();

        SubscribeEvents();

        //do the things necessary for the state
        PerformStateProcesses();

        alarm.ToggleVisibility();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (end) { return; }

        AnimateElement();
    }

    private void CheckForEndState(MNode node)
    {
        if (CheckCustomerPositionRequirements(node))
        {
            Destroy(customer.GetComponent<Sleeping>());
            UnsubscribeEvents();
            animator.SetTrigger("MoveState");
        }
    }

    private void AnimateElement()
    {
        if (alarm.UpdateAlarm(out float excess))
        {
            if (excess <= 0) { pod.PassString("customer_timeleft", "WAKE UP NOW!"); }
            else { pod.PassString("customer_timeleft", excess.ToString("F2") + "s"); }
            return;
        }

        end = true;
    }

    //private void TransferItem() { }
    private void PerformStateProcesses()
    {
        //change bed sprite
        SpriteRenderer spriteRenderer = bed.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = customer.profile.sleeping;
    }

    private bool CheckCustomerPositionRequirements(MNode node)
    {
        return node.GetComponent(checkPoint.GetType());
    }

    private void SubscribeEvents()
    {
        controller.OnMoveComplete += CheckForEndState;
    }

    private void UnsubscribeEvents()
    {
        controller.OnMoveComplete -= CheckForEndState;
    }
}
