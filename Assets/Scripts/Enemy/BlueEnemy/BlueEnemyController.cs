using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemyController : MonoBehaviour
{
    #region Serialization
    [SerializeField] GameObject _player;
    Animator _animator;
    Rigidbody2D _rb;
    [SerializeField] float _maxDistance = 5f;
    [SerializeField] float _minDistance = 1f;
    [SerializeField] float _runningSpeed = 5f;
    int movingSpeed;
    float _distance = 0f;
    bool _lookingRight = false;
    readonly int Attack = Animator.StringToHash("Attack");
    readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");


    public enum BlueEnemyStates
    {
        Idle,
        Run,
        Attack
    }

    private BlueEnemyStates _currentMovementState;
    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        movingSpeed = (int) _rb.velocity.x;
        _animator.SetInteger(MoveSpeed, movingSpeed);
        Movement();
        PlayAnimationsBasedOnState();
        
    }
    private void FixedUpdate()
    {
        FlipFace();
    }
    private void Movement()
    {
        if(_player)
            _distance=this.transform.position.x-_player.transform.position.x;
        //Idle
        if( _maxDistance < Math.Abs(_distance))
        {
            _currentMovementState = BlueEnemyStates.Idle;
            _rb.velocity = new Vector2(0f, 0f);
        }
        //Run
        else if(_minDistance < Math.Abs(_distance) && _maxDistance > Math.Abs(_distance))
        {
            
            _currentMovementState = BlueEnemyStates.Run;
            if (_lookingRight)
            {
                _rb.velocity=new Vector2(_runningSpeed, 0f);
            }
            else
            { 
                _rb.velocity=new Vector2(-_runningSpeed, 0f);
            }
        }
        // Attack
        else if(_minDistance > Math.Abs(_distance))
        {
            _currentMovementState = BlueEnemyStates.Attack;
            _rb.velocity = new Vector2(0f, 0f);
        }
    }
    private void PlayAnimationsBasedOnState()
    {
        switch (_currentMovementState)
        {
            case BlueEnemyStates.Idle:
                break;
            case BlueEnemyStates.Run:
                FindObjectOfType<AudioManager>().Play("EnemyWalk");
                break;
            case BlueEnemyStates.Attack:
                _animator.SetTrigger(Attack);
                break;
        }
    }
    private void FlipFace()
    {
        if((_distance<0f&&!_lookingRight)|| (_distance > 0f && _lookingRight)) 
        {
            Vector3 flip = this.transform.localScale;
            flip.x*=-1f;
            this.transform.localScale = flip;
            _lookingRight = !_lookingRight;
        }
    }
}
