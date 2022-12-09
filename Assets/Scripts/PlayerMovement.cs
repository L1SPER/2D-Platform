using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movingForce;
    [SerializeField] float runningForce;
    [SerializeField] float jumpingForce;

    //bool isGrounded;
    private float verticalInput;
    private float horizontalInput;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    float isGroundedRayLength = 0.25f;
    private bool facingRight = true;
        
    Rigidbody2D rb;
    public LayerMask platformLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake() 
    {
        rb=GetComponent<Rigidbody2D>();
        spriteRenderer=GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        //HandleMovement();
    }
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleJumping();
        FaceControl();
        LimitSpeed();
        //Debug.Log("Facing "+facingRight);
        Debug.Log("Velocity: "+rb.velocity.x);
        
    }

    void HandleJumping()
    {
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))&& IsGrounded())
        {
            rb.velocity = Vector2.up * jumpingForce;
        }
    }

    bool IsGrounded()
    {
        Debug.Log("IsGrounded Funct");
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, isGroundedRayLength, platformLayerMask);
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

        horizontalInput = Input.GetAxis("Horizontal")*movingForce* Time.deltaTime;
        verticalInput = Input.GetAxis("Vertical") * movingForce * Time.deltaTime;

        if(horizontalInput > 0) //Going right
        {
            rb.AddForce(new Vector2(horizontalInput, 0));
        }
        else if(horizontalInput < 0) //Going left
        {
            rb.AddForce(new Vector2(horizontalInput, 0));
        }
    }
    void FaceControl()
    {
        if (verticalInput > 0&&!facingRight) //Going right
        {
            FlipFace();
        }
        else if (verticalInput < 0&&facingRight) //Going left
        {
            FlipFace();
        }
    }
    void LimitSpeed()
    {
        if(horizontalInput>10&&Input.GetKey(KeyCode.LeftShift))
        {
            horizontalInput = 10;
        }
        else if(horizontalInput>5)
        {
            horizontalInput = 5;
        }
        if(horizontalInput<-10&&Input.GetKey(KeyCode.LeftShift))
        {
            horizontalInput = -10;
        }
        else if(horizontalInput<-5)
        {
            horizontalInput = -5;
        }
    }
    private void FlipFace()
    {
        Vector3 flip = transform.localScale;
        flip.x *= -1;
        transform.localScale = flip;
        facingRight = !facingRight;
    }
}
