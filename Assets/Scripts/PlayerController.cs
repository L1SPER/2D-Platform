using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Yapýlcaklar
    //Kayarken box collider büyük olduðundan arada boþluk oluþuyor. Kayma esnasýnda box collider küçülcek. Kayma yapmýyorsa box collider eski haline getirilecek!!!
    private float _horizontalInput;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    private Transform _rightWallCheck;
    private Transform _leftWallCheck;
    [SerializeField] private LayerMask _wallLayer;

    private bool _isGrounded;
    private bool _isWalled;
    private bool _doubleJumping;
    private bool _facingRight = true;
    private bool _isWallSliding;
    private bool _wallJumping;
    [SerializeField]  private float _xWallForce;

    //Components
    BoxCollider2D _boxCollider;
    Rigidbody2D _rb;
    Animator _animator;
    
    private MovementStates _currentMovementState;
    private MovementStates _previousMovementState;

    //These constants are used to ensure character's movement
    private const float _isGroundedRayLength = 0.25f;
    [SerializeField] private  float _wallJumpingMultiplier = 5f;
    private const float _wallJumpDuration = 0.2f;
    private const float _wallSlidingSpeed = 3.5f;
    private const float _doubleJumpingMultiplier = 1.25f;
    private const float _movingSpeed = 7.5f;
    private const float _jumpingForce = 5f;

    //Parameters
    readonly int Idle = Animator.StringToHash("Idle");
    readonly int Walk = Animator.StringToHash("Walk");
    readonly int Slide = Animator.StringToHash("Slide");
    readonly int Jump = Animator.StringToHash("Jump");
    readonly int yVelocity = Animator.StringToHash("yVelocity");
    readonly int isGrounded = Animator.StringToHash("isGrounded");
    readonly int attack1 = Animator.StringToHash("attack1");
    readonly int attack2 = Animator.StringToHash("attack2");
    readonly int attack3 = Animator.StringToHash("attack3");
    readonly int Block = Animator.StringToHash("Block");
    readonly int Roll = Animator.StringToHash("Roll");
    readonly int Hurt = Animator.StringToHash("Hurt");
    readonly int Death = Animator.StringToHash("Death");
    readonly int xVelocity = Animator.StringToHash("xVelocity");
    public enum MovementStates
    {
        Idle,
        Walking,
        Sliding,
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
        _animator=GetComponent<Animator>();
        _leftWallCheck = transform.Find("LeftWallCheck");
        _rightWallCheck = transform.Find("RightWallCheck");
    }
    void FixedUpdate()
    {
        HandleMovement();
    }
    void Update()
    {
        SetCharacterState();
        ControlSomething();
        FaceControl();
        HandleJumping();
        WallJump();
        WallSlide();
        PlayAnimationsBasedOnStates();
        //FixColliderWhileSliding();
    }
    private void LateUpdate()
    {
        FixColliderWhileSliding();
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
            if(_isWallSliding)
            {
                _currentMovementState = MovementStates.Sliding;
            }
            //else if (!_isWallSliding)
            //{
            //    _currentMovementState = MovementStates.Jumping;
            //}
        }
       
    }
    private void PlayAnimationsBasedOnStates()
    {
        switch (_currentMovementState)
        {
            case MovementStates.Idle:
                _animator.SetBool(Idle, true);
                _animator.SetBool(Slide, false);
                break;
            case MovementStates.Walking:
                _animator.SetBool(Walk, true);
                _animator.SetBool(Slide, false);
                break;
            case MovementStates.Sliding:
                _animator.SetBool(Slide, true);
                break;
            case MovementStates.Attacking:
                break;
            case MovementStates.Jumping:
                _animator.SetBool(Jump,true);
                _animator.SetBool(Slide, false);
                break;
            default:
                break;
        }
        int xvelocity = (int)_rb.velocity.x;
        int yvelocity=(int)_rb.velocity.y;
        _animator.SetBool(isGrounded, _isGrounded);
        _animator.SetInteger(xVelocity, xvelocity);
        _animator.SetFloat(yVelocity, yvelocity);
    }
    private void ControlSomething()
    {
        Debug.Log("rb : " + _rb.velocity.x);
        Debug.Log("wall jumping : " + _wallJumping); 
        Debug.Log("wall slide : " + _isWallSliding);
        Debug.Log("Current movement state : " + _currentMovementState);
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
            _currentMovementState = MovementStates.Jumping;
        }
        //double jump condition
        else if (Input.GetKeyDown(KeyCode.Space) && !IsGrounded() && _doubleJumping)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpingForce * _doubleJumpingMultiplier);
            _doubleJumping = false;
            _currentMovementState = MovementStates.Jumping;
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
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
        return _isGrounded;
    }
    private bool IsWalled()
    {
        bool leftWalled,rightWalled;
        
        leftWalled = Physics2D.OverlapCircle(_rightWallCheck.position, 0.2f, _wallLayer);
        rightWalled= Physics2D.OverlapCircle(_rightWallCheck.position, 0.2f, _wallLayer);
        if (leftWalled||rightWalled)
        {
            return true;
        }
        return false;
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
    private void FixColliderWhileSliding()
    {
        Vector2 boxColliderSizeWhileSliding = new Vector2 (0.41f, 1.19f);
        Vector2 boxColliderSizeWhileNotSliding = new Vector2(1f, 1.19f);
        Vector2 boxColliderOffsetWhileSliding = new Vector2(0.28f, 0.03f);
        Vector2 boxColliderOffsetWhileNotSliding = new Vector2(0.28f, 0.03f);
        if (_isWallSliding)
        {
            _boxCollider.size = boxColliderSizeWhileSliding;
            _boxCollider.offset = boxColliderOffsetWhileSliding;
        }
        else
        {
            _boxCollider.size = boxColliderSizeWhileNotSliding;
            _boxCollider.offset = boxColliderOffsetWhileNotSliding;
        }
    }
    private void WallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isWallSliding)
        {
            _wallJumping = true;
            Invoke("StopWallJump", _wallJumpDuration);
        }
        //wall jump
        if(_wallJumping)
            _rb.velocity = new Vector2(_xWallForce*-_horizontalInput, _jumpingForce*_wallJumpingMultiplier); 

    }
    private void StopWallJump()
    {
        _wallJumping = false;
    }
}