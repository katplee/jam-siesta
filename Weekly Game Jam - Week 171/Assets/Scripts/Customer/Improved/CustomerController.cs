using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomerController : MonoBehaviour
{
    public event Action<MNode> OnMoveComplete;

    private Customer customer = null;
    private Animator animator = null;
    private Transform movePoint = null;
    public Vector3Int startPosition { get; private set; }
    public Vector3Int endPosition { get; private set; }
    private float speed = 5f;
    private Tilemap customerTilemap = null;

    private void Awake()
    {
        //player animation-related variables
        customer = GetComponent<Customer>();
        animator = GetComponent<Animator>();

        //navigation-related variables
        movePoint = transform.GetChild(0);
        movePoint.parent = null;
        customerTilemap = TilemapManager.Instance.customerTilemap;
    }

    public void InvokeEvent()
    {
        MNode node = GameManager.Instance.RefreshNodeParent(customer);
        OnMoveComplete?.Invoke(node);
    }

    public void TransportCustomer(Transform destination)
    {
        //if the path has not been reset aka the player is moving, cancel transport operation
        if (endPosition != Vector3Int.zero) { return; }

        DefinePath(destination);

        //if the player is already in the destination
        if (endPosition == startPosition) { ResetPath(); return; }

        //change animator parameter
        animator.SetFloat("Speed", 1f);
    }

    private void DefinePath(Transform destination)
    {
        startPosition = customerTilemap.WorldToCell(transform.position);
        endPosition = customerTilemap.WorldToCell(destination.position);
    }

    public bool MoveCustomerBy(Vector3Int moveVector)
    {
        AnimateCustomer(transform.position, movePoint.position);
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            MovePointBy(moveVector);

            if (moveVector == Vector3Int.zero)
            {
                Vector3Int currentPosition = customerTilemap.WorldToCell(transform.position);
                if (currentPosition == endPosition)
                {
                    MNode node = GameManager.Instance.RefreshNodeParent(customer);
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

    private void AnimateCustomer(Vector3 from, Vector3 to)
    {
        Vector3 normalized = Vector3.Normalize(to - from);

        animator.SetFloat("Horizontal", normalized.x);
        animator.SetFloat("Vertical", normalized.y);
    }

    public void ResetPath()
    {
        endPosition = Vector3Int.zero;
    }

    /*
    public Transform movePoint;
    private float speed = 5.0f;
    public Vector3 InputVector { get; set; }
    public Vector3 Destination { get; set; }
    public List<CustomerMovementNode> pathSoFar;

    private Animator animator;

    public bool hasFinishedSequence = false;

    // Start is called before the first frame update
    private void Start(){

        movePoint.parent = null;

        animator = GetComponent<Animator>();        
    }

    void Update(){

        if(gameObject.GetComponent<CustomerDestination>() != null)
        {
            hasFinishedSequence = false;
        }

        if(gameObject.GetComponent<CustomerDestination>() == null)
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