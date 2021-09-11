using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForAlertState : StateMachineBehaviour
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
    private DresserAlarm alarm = null;

    //parameters related to completion of task
    private ItemNode checkPoint = null;
    //private Vector3Int dropoffPoint = new Vector3Int(-8, 5, 0);
    //private Luggage dropoffItem = new Luggage();
    //private Player receiver = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        customer = animator.gameObject.GetComponent<Customer>();
        controller = customer.GetComponent<CustomerController>();
        this.animator = animator;
        pod = customer.GetComponentInParent<Pod>();
        alarm = pod.GetComponentInChildren<DresserAlarm>();

        checkPoint = customer.GetComponentInParent<Bed>().GetItemNode();

        SubscribeEvents();

        //do the things necessary for the state
        PerformStateProcesses();
        
        //move the customer to the waiting for pod node
        controller.TransportCustomer(checkPoint.GetPositionInTileMap());
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UnsubscribeEvents();
    }

    private void CheckForEndState(MNode node)
    {
        animator.gameObject.AddComponent<Sleeping>();
        animator.SetTrigger("MoveState");
    }

    //private void AnimateElement() { }
    //private void TransferItem() { }

    private void PerformStateProcesses()
    {
        alarm.SetAlarmParameters(customer.GetSleepNeeded(), customer.GetSleepAllowance());
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
