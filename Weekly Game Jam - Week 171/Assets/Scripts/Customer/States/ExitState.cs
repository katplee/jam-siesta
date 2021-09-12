using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitState : StateMachineBehaviour
{
    /*
     * checkPoint : the location at which the customer must go to before a state change
     * dropoffPoint : the location at which the player must go to before receiving an item/before a state change
     * dropoffItem : the item which will be given/received by the receiver
     * receiver : the receipient of the item to be transferred
     */

    private Customer customer = null;
    private CustomerController controller = null;
    private Animator animator = null;

    //parameters related to completion of task
    private Vector3Int checkPoint = new Vector3Int(-11, -9, 0);
    //private Vector3Int dropoffPoint = new Vector3Int(-8, 5, 0);
    //private Luggage dropoffItem = new Luggage();
    //private Player receiver = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        customer = animator.gameObject.GetComponent<Customer>();
        controller = customer.GetComponent<CustomerController>();
        this.animator = animator;

        SubscribeEvents();

        //move the customer to the waiting for pod node
        controller.TransportCustomer(checkPoint);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UnsubscribeEvents();
    }

    private void CheckForEndState(MNode node)
    {
        if (CheckCustomerPositionRequirements(node))
        {
            Destroy(customer.gameObject);
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
