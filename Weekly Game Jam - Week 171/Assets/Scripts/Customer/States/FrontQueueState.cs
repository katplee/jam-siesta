using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontQueueState : StateMachineBehaviour
{
    /*
     * A state consists of at most two of the following: item transfer, element movement, element animation.
     * For example, the player moving to specific dropoffPoint and then receiving an item from a customer is one state.
     * The parameters which will be used in checking for task completion are as follows:
     * 
     * checkPoint : the location at which the customer must go to before 
     * dropoffPoint : the location at which the player must go to before receiving an item/before a state change
     * dropoffItem : the item which will be given/received by the receiver
     * receiver : the receipient of the item to be transferred
     */
    
    private Customer customer = null;
    private CustomerController controller = null;
    private Animator animator = null;

    //parameters related to completion of task
    private Vector3Int checkPoint = new Vector3Int(-12, 5, 0);
    //private Vector3Int dropoffPoint = new Vector3Int(-8, 5, 0);
    //private Luggage dropoffItem = new Luggage();
    //private Player receiver = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        customer = animator.gameObject.GetComponent<Customer>();
        controller = customer.GetComponent<CustomerController>();
        this.animator = animator;

        SubscribeEvents();

        //signals the end of necessary variable-setting
        controller.InvokeMoveCompleteEvent();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UnsubscribeEvents();    
    }

    private void CheckForEndState(MNode node)
    {
        if (CheckCustomerPositionRequirements(node))
        {
            animator.SetTrigger("MoveState");
        }
    }

    private bool CheckCustomerPositionRequirements(MNode node)
    {
        return node.GetPositionInTileMap() == checkPoint;
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
