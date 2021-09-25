using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignPodState : StateMachineBehaviour
{
    /*
     * checkPoint : the location at which the customer must go to before (can be a vector or a destination node name/item clickable)
     * dropoffPoint : the location at which the player must go to before receiving an item/before a state change
     * dropoffItem : the item which will be given/received by the receiver
     * receiver : the receipient of the item to be transferred
     */

    private Customer customer = null;
    private CustomerController controller = null;
    private CustomerPatience patience = null;
    private Animator animator = null;
    private bool end = false;

    //parameters related to completion of task
    //private Vector3Int checkPoint = new Vector3Int(-10, 0, 0);
    private Bed checkPoint = new Bed();
    //private Vector3Int dropoffPoint = new Vector3Int(-8, 5, 0);
    //private Luggage dropoffItem = new Luggage();
    //private Player receiver = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        customer = animator.gameObject.GetComponent<Customer>();
        controller = customer.controller;
        patience = customer.patience;
        this.animator = animator;

        SubscribeEvents();

        //turn the patience counter back on
        patience.SetPatienceInteractibility(true);
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
            Destroy(customer.GetComponent<Waiting>());
            UnsubscribeEvents();
            animator.SetTrigger("MoveState");
        }
    }

    private void AnimateElement()
    {
        if (patience.UpdatePatience()) { return; }

        end = true;
    }

    private bool CheckCustomerPositionRequirements(MNode node)
    {
        return customer.GetComponentInParent(checkPoint.GetType());
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
