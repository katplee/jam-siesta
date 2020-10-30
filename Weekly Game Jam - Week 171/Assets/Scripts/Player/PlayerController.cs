using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public Transform movePoint;

    private float speed = 5.0f;
    public Vector3 InputVector { get; set; }
    public Vector3Int Destination { get; set; }

    //public List<MovementNode> bestPath;

    private Animator animator;

    // Start is called before the first frame update
    private void Start(){

        movePoint.parent = null;

        animator = GetComponent<Animator>();        
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
    
}