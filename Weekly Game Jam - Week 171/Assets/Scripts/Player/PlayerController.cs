﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : Controller
{
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerController>();
            }
            return instance;
        }
    }

    public static event Action OnItemPress;
    public static event Action<MNode> OnMoveComplete;

    //private List<Transform> path = new List<Transform>();
    private Queue<Transform> path = new Queue<Transform>();


    private Player player = null;
    private Animator animator = null;
    private Transform movePoint = null;
    public Vector3Int startPosition { get; private set; }
    public Vector3Int endPosition { get; private set; }
    private MNode positionNode = null;
    private float speed = 5f;
    private Tilemap playerTilemap = null;

    private void Awake()
    {
        //player animation-related variables
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();

        //navigation-related variables
        movePoint = GetComponentInChildren<MovePoint>().transform;
        movePoint.parent = null;
        playerTilemap = transform.parent.GetComponent<Tilemap>();
    }

    private void Start()
    {
        GameManager.Instance.RefreshNodeParent(player);
    }

    public void InvokeMoveCompleteEvent()
    {
        positionNode = GameManager.Instance.RefreshNodeParent(player);
        OnMoveComplete?.Invoke(positionNode);

        //at the front of the queue is the destination where the player is currently in
        //DequeuePath(); temorarily commented out

        //if there is no more destination queued, return
        if (path.Count == 0) { return; }

        //if there is still destination queued, peek at the destination which is now at the front
        Transform nextDestination = DequeuePath();
        //go to the next path in the queue if there is a destination queued
        TransportPlayer(nextDestination);
    }

    public void TransportPlayer(Transform destination)
    {
        //add the destination to the queue and update the task panel
        //if (destinationInfo) {  }
        //if the path has not been reset aka the player is moving, cancel transport operation
        if (endPosition != Vector3Int.zero) { AddPath(destination); return; }

        //even if destination is added at a time when endPosition has already been set to zero, add to
        //the end of the queue if there is a queue
        //if(path.Count != 0) { AddPath(destination); return; }
        
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

    public void TransportPlayer(Vector3Int destination)
    {
        //if the path has not been reset aka the player is moving, cancel transport operation
        if (endPosition != Vector3Int.zero) { return; }

        DefinePath(destination);

        //if the player is already in the destination
        if (endPosition == startPosition) { ResetPath(); return; }

        //change animator parameter
        animator.SetFloat("Speed", 1f);
    }

    private void DefinePath(Vector3Int destination)
    {
        startPosition = playerTilemap.WorldToCell(transform.position);
        endPosition = destination;
    }

    public bool MovePlayerBy(Vector3Int moveVector)
    {
        AnimatePlayer(transform.position, movePoint.position);
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            MovePointBy(moveVector);

            if (moveVector == Vector3Int.zero)
            {
                Vector3Int currentPosition = playerTilemap.WorldToCell(transform.position);
                if (currentPosition == endPosition)
                {
                    animator.SetFloat("Speed", 0f);
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
        InvokeMoveCompleteEvent();
    }

    //private void AddPath(Transform path, DestinationScriptable destination)
    private void AddPath(Transform path)

    {
        this.path.Enqueue(path);
        //UpdateTaskPanel(destination);
    }

    private void UpdateTaskPanel(DestinationScriptable destination)
    {
        if (!destination) { return; }
        TaskPanel.Instance.GenerateInstance(destination);
    }

    public Transform DequeuePath()
    {
        Transform next = path.Dequeue();
        return next;
    }

    /* temorarily commented out
    private Transform IndexOf(Transform destination, List<GameObject> list)
    {
        int i = 0;

        foreach (GameObject obj in list)
        {
            if (obj == item) { return i; }
            i++;
        }

        return -1;
    }
    */

    public bool IsMoving()
    {
        bool moving = (endPosition != Vector3Int.zero) ? true : false;
        return moving;
    }
}