using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movingSpeed;
    [SerializeField] float jumpingForce;
    private bool doubleJump;
    
    private float horizontalInput;

    private BoxCollider2D boxCollider;
    float isGroundedRayLength = 0.25f;
    private bool facingRight = true;
   
    Rigidbody2D rb;
    public LayerMask groundLayer;
    private int jumpCounterValue = 0;
    private int jumpCounter;

    private MovementStates currentMovementState;
    private MovementStates previousMovementState;
    public enum MovementStates
    {
        Idle,
        Walking,
        Attacking,
        Jumping
    }
    void Start()
    {
        jumpCounter = jumpCounterValue;
        currentMovementState = MovementStates.Idle;
    }
    void Awake() 
    {
        rb=GetComponent<Rigidbody2D>();
        boxCollider=GetComponent<BoxCollider2D>();
    }
    void FixedUpdate()
    {
        HandleMovement();
    }
    void Update()
    {
        ControlSomething();
        HandleJumping();
        FaceControl();
        SetCharacterState();
    }

    private void ControlSomething()
    {
        Debug.Log("Jumping velocity"+rb.velocity.x);
        Debug.Log("Double jump :"+ doubleJump);
        Debug.Log("MovementState:" + currentMovementState);

    }

    void HandleJumping()
    {
        //first jump
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingForce);
            doubleJump = true;
        }
        //double jump condition
        else if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))   && !IsGrounded() && doubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingForce*1.25f);
            doubleJump = false;
        }
    }

    bool IsGrounded()
    {
        Debug.Log("IsGrounded Funct");
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, isGroundedRayLength, groundLayer);
        Color rayColor;
        if (raycastHit2D.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y + isGroundedRayLength), rayColor);
        if (raycastHit2D.collider != null)
        {
            Debug.Log("We are colliding with" + raycastHit2D.collider);
        }
        return raycastHit2D.collider != null;
    }

    void HandleMovement()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        horizontalInput = Input.GetAxis("Horizontal") ;

        if(Input.GetKey(KeyCode.D))//Walking right
        {
            rb.velocity = new Vector2(horizontalInput * movingSpeed, rb.velocity.y);
        }
        else if(Input.GetKey(KeyCode.A))//Walking left
        {
            rb.velocity=new Vector2(horizontalInput*movingSpeed,rb.velocity.y);
        }
    }
    void FaceControl()
    {
        if (horizontalInput > 0&&!facingRight) //Going right
        {
            FlipFace();
        }
        else if (horizontalInput< 0&&facingRight) //Going left
        {
            FlipFace();
        }
    }
    private void FlipFace()
    {
        Vector3 flip = transform.localScale;
        flip.x *= -1;
        transform.localScale = flip;
        facingRight = !facingRight;
    }

    private void SetCharacterState() 
    {
        if(IsGrounded())
        {
            if(rb.velocity.x==0)
            {
                currentMovementState = MovementStates.Idle;
            }
            else
            {
                currentMovementState = MovementStates.Walking;
            }
        }
        else
        {
            currentMovementState = MovementStates.Jumping;
        }
    }
}
