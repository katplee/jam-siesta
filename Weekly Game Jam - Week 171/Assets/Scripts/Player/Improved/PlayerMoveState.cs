using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMoveState : StateMachineBehaviour
{
    private Stack<Vector3Int> finalPath = null;
    private Vector3Int currentPosition = new Vector3Int();

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SubscribeEvents();

        //get inputs for navigation
        Vector3Int startPosition = PlayerController.Instance.startPosition;
        Vector3Int endPosition = PlayerController.Instance.endPosition;
        currentPosition = startPosition;
        Tilemap playerTileMap = TilemapManager.Instance.playerTileMap;

        AstarAlgorithm navigation = new AstarAlgorithm(startPosition, endPosition, playerTileMap);
        finalPath = navigation.FindPath();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(finalPath == null) { return; }

        Vector3Int moveVector = ComputeMoveVector(currentPosition);
        if (PlayerController.Instance.MovePlayerBy(moveVector))
        {
            if(finalPath.Count == 0) { return; }

            currentPosition = finalPath.Pop();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController.Instance.ResetPath();

        UnsubscribeEvents();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private Vector3Int ComputeMoveVector(Vector3Int currentPosition)
    {
        if(finalPath.Count == 0) { return Vector3Int.zero; }

        return finalPath.Peek() - currentPosition;
    }

    private void ExitMoveState()
    {
        AnimatorManager.Animator.SetFloat("Speed", 0f);
    }

    private void SubscribeEvents()
    {
        PlayerController.OnMoveComplete += ExitMoveState;
    }

    private void UnsubscribeEvents()
    {
        PlayerController.OnMoveComplete -= ExitMoveState;
    }
}
