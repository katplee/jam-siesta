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
    private DresserAlarm alarm = null;
    private bool end = false;

    //parameters related to completion of task
    //private Vector3Int checkPoint = new Vector3Int(-12, 5, 0);
    //private Vector3Int dropoffPoint = new Vector3Int(-8, 5, 0);
    //private Luggage dropoffItem = new Luggage();
    //private Player receiver = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        customer = animator.gameObject.GetComponent<Customer>();
        controller = customer.GetComponent<CustomerController>();
        this.animator = animator;
        alarm = customer.GetComponentInParent<DresserAlarm>();
        alarm.ToggleVisibility();

        SubscribeEvents();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (end) { return; }

        AnimateElement();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UnsubscribeEvents();
    }

    private void CheckForEndState(MNode node)
    {
        if (CheckCustomerPositionRequirements(node))
        {
            Destroy(customer.GetComponent<Waiting>());
            animator.SetTrigger("MoveState");
        }
    }

    private void AnimateElement()
    {
        if (alarm.UpdateAlarm()) { return; }
        
        end = true;

        controller.InvokeMoveCompleteEvent();
    }

    //private void TransferItem() { }
    //private void PerformStateProcesses() { }

    private bool CheckCustomerPositionRequirements(MNode node)
    {
        //return customer.GetComponentInParent(checkPoint.GetType());
        return true;
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
