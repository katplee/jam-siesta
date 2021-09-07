using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPajamasState : StateMachineBehaviour
{
    /*
     * checkPoint : the location at which the customer must go to before the state changes
     * dropoffPoint : the location at which the player must go to before receiving an item/before a state change
     * dropoffItem : the item which will be given/received by the receiver
     * receiver : the receipient of the item to be transferred
     */

    private Animator animator = null;
    
    //parameters related to completion of task
    //private Vector3Int checkPoint = new Vector3Int(-12, 5, 0);
    private Vector3Int dropoffPoint = new Vector3Int(-10, 3, 0);
    private Pajamas pickUpItem = new Pajamas();
    private Player receiver = null;

    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SubscribeEvents();

        this.animator = animator;
        receiver = Player.Instance;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UnsubscribeEvents();
    }

    private void CheckForEndState(MNode node)
    {
        if (CheckPlayerPositionRequirements(node))
        {
            bool end = TransferItem();
            if (end) { animator.SetTrigger("MoveState"); }
        }
    }

    private bool CheckPlayerPositionRequirements(MNode node)
    {
        return node.GetPositionInTileMap() == dropoffPoint;
    }

    private bool TransferItem()
    {
        //ItemClickable giver = animator.gameObject.GetComponent<Customer>();
        //bool transfered = giver.GiveItemTo(receiver, dropoffItem);

        return false;
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
