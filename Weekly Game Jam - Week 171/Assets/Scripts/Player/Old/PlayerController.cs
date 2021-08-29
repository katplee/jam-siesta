using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : Singleton<PlayerController>
{
    public static event Action OnItemPress;
    public static event Action OnMoveComplete;

    private Animator animator = null;

    private Transform movePoint = null;
    public Vector3Int startPosition { get; private set; }
    public Vector3Int endPosition { get; private set; }
    private float speed = 5f;
    private Tilemap playerTilemap = null; 

    private void Awake()
    {
        //player animation-related variables
        animator = GetComponent<Animator>();

        //navigation-related variables
        movePoint = transform.GetChild(0);
        movePoint.parent = null;
        playerTilemap = transform.parent.GetComponent<Tilemap>();
    }

    public void AnimatePlayer(Transform destination)
    {
        DefinePath(destination);

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
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            MovePointBy(moveVector);

            if(moveVector == Vector3Int.zero)
            {
                Vector3Int currentPosition = playerTilemap.WorldToCell(transform.position);
                if(currentPosition == endPosition)
                {
                    OnMoveComplete?.Invoke();
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

    public void ResetPath()
    {
        endPosition = Vector3Int.zero;
    }




    /*
    private Transform movePoint;

    private float speed = 5.0f;
    public Vector3 InputVector { get; set; }
    public Vector3Int Destination { get; set; }

    private Animator animator;

    private void Awake()
    {
        movePoint = transform.GetChild(0);
        animator = GetComponent<Animator>();        
    }

    private void Start()
    {
        movePoint.parent = null;
    }

    void Update(){

        if(gameObject.GetComponent<PlayerDestination>() == null)
        {
            InputVector = new Vector3(0f, 0f, 0f);
        }

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);        
        
        animator.SetFloat("Horizontal", InputVector.x);
        animator.SetFloat("Vertical", InputVector.y);
        animator.SetFloat("Speed", InputVector.sqrMagnitude);
        
        if(Vector3.Distance(movePoint.position, Destination) > 0.1f)
        {
            if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
            {
                Move(InputVector);
            }
        }
    }

    private void Move(Vector3 inputVector){
        movePoint.position += inputVector;
    }
    */
}