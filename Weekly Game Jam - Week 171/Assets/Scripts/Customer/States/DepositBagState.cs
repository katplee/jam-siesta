using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositBagState : StateMachineBehaviour
{
    private Customer customer = null;
    private CustomerController controller = null;
    private CustomerPatience patience = null;
    private CustomerSatisfaction satisfaction = null;
    private Animator animator = null;
    private bool end = false;
    
    //parameters related to completion of task
    private Vector3Int dropoffPoint = new Vector3Int(-6, 5, 0);
    private Luggage dropoffItem = null;
    private Player receiver = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        customer = animator.gameObject.GetComponent<Customer>();
        controller = customer.controller;
        patience = customer.patience;
        satisfaction = customer.satisfaction;
        this.animator = animator;
        receiver = Player.Instance;

        SubscribeEvents();
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
            //update customer satisfaction
            float grade = patience.ResetPatience();
            satisfaction.ComputeSatisfaction(grade, ticketValue);

            bool end = TransferItem();
            if (end) 
            {
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
