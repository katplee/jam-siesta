using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagDepositState : StateMachineBehaviour
{
    private Animator animator = null;
    
    //parameters related to completion of task
    private Vector3Int dropoffPoint = new Vector3Int(-8, 5, 0);
    private Luggage dropoffItem = new Luggage();
    private Player receiver = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
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
        Customer giver = animator.gameObject.GetComponent<Customer>();
        bool transfered = giver.GiveItemTo(receiver, dropoffItem);

        return transfered;
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
