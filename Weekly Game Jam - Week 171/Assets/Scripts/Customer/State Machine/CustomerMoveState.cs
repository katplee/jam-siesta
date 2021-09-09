using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomerMoveState : StateMachineBehaviour
{
    private Customer customer = null;
    private CustomerController controller = null;
    private Animator animator = null;
    private Stack<Vector3Int> finalPath = null;
    private Vector3Int currentPosition = new Vector3Int();

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        customer = animator.gameObject.GetComponent<Customer>();
        controller = customer.GetComponent<CustomerController>();
        this.animator = animator;

        //get inputs for navigation
        Vector3Int startPosition = controller.startPosition;
        Vector3Int endPosition = controller.endPosition;
        currentPosition = startPosition;
        Tilemap customerTilemap = TilemapManager.Instance.customerTilemap;

        AstarAlgorithm navigation = new AstarAlgorithm(startPosition, endPosition, customerTilemap);
        finalPath = navigation.FindPath();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (finalPath == null) { return; }

        Vector3Int moveVector = ComputeMoveVector(currentPosition);
        if (controller.MoveCustomerBy(moveVector))
        {
            currentPosition = finalPath.Pop();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.ResetPath();
    }

    private Vector3Int ComputeMoveVector(Vector3Int currentPosition)
    {
        if (finalPath.Count == 0) { return Vector3Int.zero; }

        return finalPath.Peek() - currentPosition;
    }
}
