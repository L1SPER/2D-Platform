using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PinkEnemyController : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] float _attackDistance = 10f;
    
    float _distance = 0f;
    public bool _lookingRight = false;
    bool _isAttacking;
    Animator _animator;
    readonly int Attack = Animator.StringToHash("Attack");
    PinkEnemyCombat _pinkEnemyCombat;

    public enum PinkEnemyStates
    {
        Idle,
        Attack
    }
    private PinkEnemyStates _currentMovementState;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _pinkEnemyCombat=GetComponent<PinkEnemyCombat>();
    }
    private void Update()
    {
        Movement();
        PlayAnimationsBasedOnState();
        _animator.SetBool(Attack, _isAttacking);
    }
    private void FixedUpdate()
    {
        FlipFace();
    }
    private void Movement()
    {
        if (_player)
            _distance = this.transform.position.x - _player.transform.position.x;
        //Idle
        if(_attackDistance<Mathf.Abs(_distance))
        {
            _currentMovementState = PinkEnemyStates.Idle;
        }
        //Attack
        else if(_attackDistance > Mathf.Abs(_distance))
        {
            _currentMovementState= PinkEnemyStates.Attack;
            _pinkEnemyCombat.Shoot();
        }
    }
    private void PlayAnimationsBasedOnState()
    {
        switch (_currentMovementState)
        {
            case PinkEnemyStates.Idle:
                _isAttacking = false;
                break;
            case PinkEnemyStates.Attack:
                _isAttacking = true;
                break;
            default:
                break;
        }
    }
    private void FlipFace()
    {
        if ((_distance < 0f && !_lookingRight) || (_distance > 0f && _lookingRight))
        {
            Vector3 flip = this.transform.localScale;
            flip.x *= -1f;
            this.transform.localScale = flip;
            _lookingRight = !_lookingRight;
        }
    }
}
