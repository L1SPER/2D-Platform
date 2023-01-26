using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _horizontalInput;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    private Transform _wallCheck;
    //private Transform _leftWallCheck;
    [SerializeField] private LayerMask _wallLayer;

    //Bools
    private bool _isGrounded;
    private bool _isWalled;
    private bool _doubleJumping;
    private bool _facingRight = true;
    private bool _isWallSliding;
    private bool _wallJumping;
    private bool _isAttacking;
    private bool _isRolling;
    //Components
    Rigidbody2D _rb;
    Animator _animator;
    BoxCollider2D boxCollider;
    //Scripts
    PlayerAttack _playerAttack;

    //MovementStates
    private MovementStates _currentMovementState;
    private MovementStates _previousMovementState;

    //These constants are used to ensure character's movement
    private const float _isGroundedRayLength = 0.25f;
    private  float _wallJumpingMultiplier = 5f;
    private const float _wallJumpDuration = 1f;
    private const float _wallSlidingSpeed = 3.5f;
    private const float _doubleJumpingMultiplier = 1.25f;
    private const float _movingSpeed = 7.5f;
    private const float _rollingSpeed = 10f;
    private const float _jumpingForce = 5f;
    private const float _attackingTime = 0.5f;
    private float _xWallForce;
    
    //Parameters
    readonly int Idle = Animator.StringToHash("Idle");
    readonly int Walk = Animator.StringToHash("Walk");
    readonly int Slide = Animator.StringToHash("Slide");
    readonly int Jump = Animator.StringToHash("Jump");
    readonly int yVelocity = Animator.StringToHash("yVelocity");
    readonly int isGrounded = Animator.StringToHash("isGrounded");
    
    readonly int Block = Animator.StringToHash("Block");
    readonly int Roll = Animator.StringToHash("Roll");
    readonly int Hurt = Animator.StringToHash("Hurt");
    readonly int Death = Animator.StringToHash("Death");
    readonly int xVelocity = Animator.StringToHash("xVelocity");

    KeyCode attackButton = KeyCode.Mouse0;
    KeyCode jumpButton = KeyCode.Space;
    KeyCode rollButton = KeyCode.C;

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
        _animator=GetComponent<Animator>();
        _wallCheck = transform.Find("WallCheck");
        _playerAttack=GetComponent<PlayerAttack>();
        boxCollider=GetComponent<BoxCollider2D>();
    }
    void FixedUpdate()
    {
        HandleMovement();
    }
    void Update()
    {
        ControlSomething();
        FaceControl();
        HandleJumping();
        WallActions();
        SetCharacterState();
        PlayAnimationsBasedOnStates();
        StartAttack();
        HandleRolling();
    }
    /// <summary>
    /// Sets character states
    /// </summary>
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
            else
            {
                _currentMovementState = MovementStates.Jumping;
            }
        }
    }
    /// <summary>
    /// Plays animation based on states
    /// </summary>
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
        Debug.Log("wall slide : " + _isWallSliding);
        Debug.Log("Current movement state : " + _currentMovementState);
    }
    private void HandleMovement()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        _horizontalInput = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(_horizontalInput * _movingSpeed, _rb.velocity.y);

        if(_horizontalInput!= 0&&IsGrounded())//Fixed walk sound due to player input
        {
            FindObjectOfType<AudioManager>().Play("PlayerWalk");
        }
    }
    /// <summary>
    /// Controls direction of face according to speed of player 
    /// </summary>
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
    /// <summary>
    /// Flips face
    /// </summary>
    private void FlipFace()
    {
        Vector3 flip = transform.localScale;
        flip.x *= -1;
        transform.localScale = flip;
        _facingRight = !_facingRight;
    }
    /// <summary>
    /// Handles jumping mechanisms
    /// </summary>
    private void HandleJumping()
    {
        //First jump
        if (Input.GetKeyDown(jumpButton) && IsGrounded())
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpingForce);
            _doubleJumping = true;
            FindObjectOfType<AudioManager>().Play("PlayerJump");
        }
        //double jump condition
        else if (Input.GetKeyDown(jumpButton) && !IsGrounded() && _doubleJumping)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpingForce * _doubleJumpingMultiplier);
            _doubleJumping = false;
            FindObjectOfType<AudioManager>().Play("PlayerJump");
        }
    }
    /// <summary>
    /// Checks  whether the player touches the ground
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Checks whether the player touches the wall
    /// </summary>
    /// <returns></returns>
    private bool IsWalled()
    {
        _isWalled= Physics2D.OverlapCircle(_wallCheck.position, 0.2f, _wallLayer);
        return _isWalled;
    }
    /// <summary>
    /// Wall actions include both wall sliding and wall jumping
    /// </summary>
    private void WallActions()
    {
        if (IsWalled() && !IsGrounded() && _horizontalInput != 0f)
            _isWallSliding = true;
        else
            _isWallSliding = false;

        //if(_isWallSliding&&!_wallJumping)
        //    _rb.velocity = new Vector2(_rb.velocity.x, -_wallSlidingSpeed);//Mathf.Clamp(_rb.velocity.y,-_wallSlidingSpeed,float.MaxValue));
        if(_isWallSliding) 
        {
            if (Input.GetKeyDown(jumpButton))
            {
                _wallJumping = true;
                Invoke("StopWallJump", _wallJumpDuration);
            }
            else if(!_wallJumping)
            {
                Debug.Log("Wall jumping false");
                _rb.velocity = new Vector2(_rb.velocity.x, -_wallSlidingSpeed);
            }
        }
        if(_wallJumping)
        {
            Debug.Log("Wall jumping true");
            //_rb.AddForce(new Vector2(-_horizontalInput*_xWallForce, _wallJumpingMultiplier));
            _rb.velocity = new Vector2(_xWallForce * -_horizontalInput,  _wallJumpingMultiplier);
        }
    }
    /// <summary>
    /// Stops wall jumping by making _wallJumping bool false
    /// </summary>
    private void StopWallJump()
    {
        _wallJumping = false;
    }
    /// <summary>
    /// Starts to attack
    /// </summary>
    private void StartAttack()
    {
        if(Input.GetKeyDown(attackButton)&&IsGrounded()) 
        {
            _playerAttack.Attack();
            _currentMovementState = MovementStates.Attacking;
            _isAttacking= true;
            FindObjectOfType<AudioManager>().Play("PlayerAttack");
            Invoke("StopAttacking", _attackingTime);
        }
    }
    /// <summary>
    /// Stops attacking by making _isAttacking bool false
    /// </summary>
    private void StopAttacking()
    {
        _isAttacking= false;
    }
    /// <summary>
    /// Handles rolling mechanism
    /// </summary>
    private void HandleRolling()
    {
        if (Input.GetKeyDown(rollButton)&&!_isRolling&&IsGrounded()&&_rb.velocity.x!=0)
        {
            Invoke("StopRolling", 1f);
            _rb.velocity=new Vector2(_rollingSpeed, _rb.velocity.y);
            _animator.SetTrigger(Roll);
            _isRolling = true;
            boxCollider.enabled= false;
        }
    }
    /// <summary>
    /// Stops rolling by making _isRolling bool false
    /// </summary>
    private void StopRolling()
    {
        _isRolling = false;
        boxCollider.enabled = true;
    }
}