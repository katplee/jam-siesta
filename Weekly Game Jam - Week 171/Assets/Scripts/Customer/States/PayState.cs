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
    private PlayerNode playerNode = null;
    private CustomerPatience patience = null;
    private CustomerSatisfaction satisfaction = null;
    private SpriteRenderer renderer = null;
    private Animator animator = null;
    private bool end = false;

    //parameters related to completion of task
    //private Vector3Int checkPoint = new Vector3Int(-12, 5, 0);
    private Vector3Int dropoffPoint = new Vector3Int(-10, -6, 0);
    private Luggage pickUpItem = null;
    private Customer receiver = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        customer = animator.gameObject.GetComponent<Customer>();
        playerNode = customer.GetComponentInChildren<PlayerNode>();
        patience = customer.patience;
        satisfaction = customer.satisfaction;
        renderer = customer.GetComponent<SpriteRenderer>();
        this.animator = animator;
        receiver = customer;

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
        if (CheckPlayerPositionRequirements(node, out float ticketValue))
        {
            bool end = TransferItem();
            float _ticketValue = playerNode.GetTicketValue();
            if (end || _ticketValue != 0) 
            {
                //update customer satisfaction
                float grade = patience.ResetPatience();
                satisfaction.ComputeSatisfaction(grade, ticketValue);
                satisfaction.ComputeSatisfaction(-1, _ticketValue);

                renderer.enabled = false;
                UnsubscribeEvents();
                animator.SetTrigger("MoveState");
            }
        }
    }

    private void AnimateElement()
    {
        if (patience.UpdatePatience()) { return; }

        end = true;
    }

    private bool CheckPlayerPositionRequirements(MNode node, out float ticketValue)
    {
        ticketValue = node.GetTicketValue();
        return node.GetPositionInTileMap() == dropoffPoint;
    }

    private bool TransferItem()
    {
        Player giver = Player.Instance;
        bool transfered = giver.GiveItemTo(receiver, pickUpItem);

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
