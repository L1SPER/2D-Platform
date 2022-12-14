using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _horizontalInput;
    private float _movingSpeed = 7.5f;
    private float _jumpingForce = 5f;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private LayerMask _wallLayer;

    private bool _doubleJumping;
    private float _doubleJumpingMultiplier = 1.25f;
    private bool _facingRight = true;
    private bool _isWallSliding;
    private float _wallSlidingSpeed = 3.5f;

    private bool _wallJumping;
    [SerializeField]  private float _xWallForce;
    private float _wallJumpDuration = 0.2f;
    [SerializeField] private float _wallJumpingMultiplier = 5f;

    private BoxCollider2D _boxCollider;
    private const float _isGroundedRayLength = 0.25f;

    private Rigidbody2D _rb;

    private MovementStates _currentMovementState;
    private MovementStates _previousMovementState;
    public enum MovementStates
    {
        Idle,
        Walking,
        Attacking,
        Jumping
    }
    void Start()
    {
        _currentMovementState = MovementStates.Idle;
    }
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }
    void FixedUpdate()
    {
        HandleMovement();
        WallSlide();
    }
    void Update()
    {
        SetCharacterState();
        ControlSomething();
        FaceControl();
        HandleJumping();
        WallJump();
    }

    private void SetCharacterState()
    {
        if (IsGrounded())
        {
            if (_rb.velocity.x == 0)
            {
                _currentMovementState = MovementStates.Idle;
            }
            else
            {
                _currentMovementState = MovementStates.Walking;
            }
        }
        else
        {
            _currentMovementState = MovementStates.Jumping;
        }
    }
    private void ControlSomething()
    {
        Debug.Log("rb : " + _rb.velocity.x);
        Debug.Log("wall jumping : " + _wallJumping); 
        Debug.Log("wall slide : " + _isWallSliding); 
    }
    private void HandleMovement()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        _horizontalInput = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(_horizontalInput * _movingSpeed, _rb.velocity.y);

       

        //if (Input.GetKeyDown(KeyCode.RightArrow))//Walking right
        //{
        //    _rb.velocity = new Vector2(_horizontalInput * _movingSpeed, _rb.velocity.y);
        //}
        //else if(Input.GetKeyDown(KeyCode.LeftArrow))//Walking left
        //{
        //    _rb.velocity=new Vector2(_horizontalInput*_movingSpeed,_rb.velocity.y);
        //}
    }
    private void FaceControl()
    {
        if (_horizontalInput > 0 && !_facingRight) //Going right
        {
            FlipFace();
        }
        else if (_horizontalInput < 0 && _facingRight) //Going left
        {
            FlipFace();
        }
    }
    private void FlipFace()
    {
        Vector3 flip = transform.localScale;
        flip.x *= -1;
        transform.localScale = flip;
        _facingRight = !_facingRight;
    }
    private void HandleJumping()
    {
        //first jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpingForce);
            _doubleJumping = true;
        }
        //double jump condition
        else if (Input.GetKeyDown(KeyCode.Space) && !IsGrounded() && _doubleJumping)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpingForce * _doubleJumpingMultiplier);
            _doubleJumping = false;
        }
        //else if (_isWallSliding)
        //{
        //    if(Input.GetKeyDown(KeyCode.Space))
        //    {
        //        _wallJumping = true;
        //        _rb.velocity = new Vector2(-_rb.velocity.x* _wallJumpingMultiplier, _jumpingForce);
        //        Invoke("StopWallJump", _wallJumpDuration);
        //    }
        //}
    }
    private bool IsGrounded()
    {
        //Debug.Log("IsGrounded Funct");
        //RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, isGroundedRayLength, groundLayer);
        //Color rayColor;
        //if (raycastHit2D.collider != null)
        //{
        //    rayColor = Color.green;
        //}
        //else
        //{
        //    rayColor = Color.red;
        //}

        //Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y + isGroundedRayLength), rayColor);
        //if (raycastHit2D.collider != null)
        //{
        //    Debug.Log("We are colliding with" + raycastHit2D.collider);
        //}
        //return raycastHit2D.collider != null;
        return Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(_wallCheck.position, 0.2f, _wallLayer);
    }
    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && _horizontalInput != 0f)
            _isWallSliding = true;
        else
            _isWallSliding = false;

        if(_isWallSliding)
            _rb.velocity = new Vector2(_rb.velocity.x, -_wallSlidingSpeed); //Mathf.Clamp(_rb.velocity.y,-_wallSlidingSpeed,float.MaxValue));
    }
    private void WallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isWallSliding)
        {
            _wallJumping = true;
            Invoke("StopWallJump", _wallJumpDuration);
        }

        if(_wallJumping)
            _rb.velocity = new Vector2(_xWallForce*-_horizontalInput, _jumpingForce); 

    }
    private void StopWallJump()
    {
        _wallJumping = false;
    }
}