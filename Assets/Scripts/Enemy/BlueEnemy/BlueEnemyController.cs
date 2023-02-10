using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemyController : MonoBehaviour
{
    [SerializeField] GameObject _player;
    Animator _animator;
    Rigidbody2D _rb;
    [SerializeField] float _maxDistance = 5f;
    [SerializeField] float _minDistance = 1f;
    float _distance = 0f;
    float _runningSpeed = 500f;
    bool _lookingRight = false;
    public enum BlueEnemyStates
    {
        Idle,
        Run,
        Attack
    }

    private BlueEnemyStates _currentMovementState;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Movement();
        PlayAnimationsBasedOnState();
    }
    private void Movement()
    {
        _distance=this.transform.position.x-_player.transform.position.x;
        //Idle
        if( _maxDistance < _distance)
        {
            _currentMovementState = BlueEnemyStates.Idle;
            _rb.velocity = new Vector2(0f, 0f);
        }
        //Run
        else if(_minDistance < _distance && _maxDistance > _distance)
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
        else if(_minDistance>_distance)
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
                break;
            case BlueEnemyStates.Attack:
                break;
        }
    }
}
