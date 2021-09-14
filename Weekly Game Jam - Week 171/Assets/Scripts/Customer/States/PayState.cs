using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayState : StateMachineBehaviour
{
    /*
     * checkPoint : the location at which the customer must go to before the state changes
     * dropoffPoint : the location at which the player must go to before receiving an item/before a state change
     * dropoffItem : the item which will be given/received by the receiver
     * receiver : the receipient of the item to be transferred
     */

    private Customer customer = null;
    private CustomerPatience patience = null;
    private SpriteRenderer renderer = null;
    private Animator animator = null;

    //parameters related to completion of task
    //private Vector3Int checkPoint = new Vector3Int(-12, 5, 0);
    private Vector3Int dropoffPoint = new Vector3Int(-10, -6, 0);
    private Pajamas pickUpItem = null;
    private Player receiver = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        customer = animator.gameObject.GetComponent<Customer>();
        patience = customer.GetComponent<CustomerPatience>();
        renderer = customer.GetComponent<SpriteRenderer>();
        this.animator = animator;
        receiver = Player.Instance;

        SubscribeEvents();
    }

    private void CheckForEndState(MNode node)
    {
        if (CheckPlayerPositionRequirements(node))
        {
            bool end = TransferItem();
            if (end) 
            {
                renderer.enabled = false;
                patience.DeactivatePatience();
                UnsubscribeEvents();
                animator.SetTrigger("MoveState");
            }
        }
    }

    private bool CheckPlayerPositionRequirements(MNode node)
    {
        return node.GetPositionInTileMap() == dropoffPoint;
    }

    private bool TransferItem()
    {
        Customer giver = customer;
        //bool transfered = giver.GiveItemTo(receiver, pickUpItem);

        return true;
    }

    private void SubscribeEvents()
    {
        PlayerController.OnMoveComplete += CheckForEndState;
    }

    private void UnsubscribeEvents()
    {
        PlayerController.OnMoveComplete -= CheckForEndState;
    }
}
