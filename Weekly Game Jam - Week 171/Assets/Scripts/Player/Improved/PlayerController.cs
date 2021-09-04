using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : Singleton<PlayerController>
{
    public static event Action OnItemPress;
    public static event Action<MNode> OnMoveComplete;

    private Player player = null;
    private Animator animator = null;
    private Transform movePoint = null;
    public Vector3Int startPosition { get; private set; }
    public Vector3Int endPosition { get; private set; }
    private float speed = 5f;
    private Tilemap playerTilemap = null; 

    private void Awake()
    {
        //player animation-related variables
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();

        //navigation-related variables
        movePoint = transform.GetChild(0);
        movePoint.parent = null;
        playerTilemap = transform.parent.GetComponent<Tilemap>();
    }

    public void TransportPlayer(Transform destination)
    {

        //if the path has not been reset aka the player is moving, cancel transport operation
        if(endPosition != Vector3Int.zero) { return; }

        DefinePath(destination);

        //if the player is already in the destination
        if (endPosition == startPosition) { ResetPath(); return; }

        //change animator parameter
        animator.SetFloat("Speed", 1f);
    }

    private void DefinePath(Transform destination)
    {
        startPosition = playerTilemap.WorldToCell(transform.position);
        endPosition = playerTilemap.WorldToCell(destination.position);
    }

    public bool MovePlayerBy(Vector3Int moveVector)
    {
        AnimatePlayer(transform.position, movePoint.position);
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            MovePointBy(moveVector);

            if(moveVector == Vector3Int.zero)
            {
                Vector3Int currentPosition = playerTilemap.WorldToCell(transform.position);
                if(currentPosition == endPosition)
                {
                    MNode node = GameManager.Instance.RefreshNodeParent(player);
                    OnMoveComplete?.Invoke(node);
                    return false;
                }
            }

            return true;
            //move point is moving towards current destination
            //stack containing path must pop
        }

        return false;
    }

    private void MovePointBy(Vector3 moveDistance)
    {
        movePoint.position += moveDistance;
    }

    private void AnimatePlayer(Vector3 from, Vector3 to)
    {
        Vector3 normalized = Vector3.Normalize(to - from);
        
        animator.SetFloat("Horizontal", normalized.x);
        animator.SetFloat("Vertical", normalized.y);
    }

    public void ResetPath()
    {
        endPosition = Vector3Int.zero;
    }
}