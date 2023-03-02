using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private int _attackIndex;
    private float _timer;
    private float _attackTime;
    private float _comboTime;
    private bool _comboStarted;
    private float _resetAttackTime = 1f;
    private Animator _animator;

    [SerializeField] private int _attackDamage = 1;
    [SerializeField] private float _attackRange;
    [SerializeField] GameObject _attackPoint;
    [SerializeField] LayerMask _targetLayer;

    readonly int attack1 = Animator.StringToHash("attack1");
    readonly int attack2 = Animator.StringToHash("attack2");
    readonly int attack3 = Animator.StringToHash("attack3");

    private void Awake()
    {
        _animator= GetComponent<Animator>();
    }
    private void Start()
    {
        _timer = 0;
        _attackIndex = 0;
        _comboStarted = false;
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        //Timer always increases. If it reaches maxValue, it must be 0 to attack
        if(_timer==float.MaxValue) 
        { 
            _timer= 0; 
        }
    }
    /// <summary>
    /// Function that includes combos and attacks against time
    /// </summary>
    public void Attack()
    {
        CheckHit();
        StartCombo();
    }
    /// <summary>
    /// If we hit enemies, they will take damage.
    /// </summary>
    private void CheckHit()
    {
        Collider2D[] hitResults = Physics2D.OverlapCircleAll(_attackPoint.transform.position, _attackRange, _targetLayer);
        if (hitResults == null)
            return;

        foreach (Collider2D hit in hitResults)
        {
            if (hit.GetComponent<IDamageable<int>>() != null)
            {
                hit.GetComponent<IDamageable<int>>().TakeDamage(_attackDamage);
            }
        }
    }
    /// <summary>
    /// Starts combo
    /// </summary>
    private void StartCombo()
    {
        //First attack started
        if (!_comboStarted)
        {
            Invoke("ResetAttack", _resetAttackTime);
            _comboStarted = true;
            _attackIndex++;

            _animator.SetTrigger(attack1);
            _attackTime = _timer;
        }
        //Combo started
        else
        {
            _comboTime = _timer;
            if (_comboTime < _attackTime + 0.3f && _comboTime > _attackTime && _attackIndex == 1)
            {
                _attackIndex++;
                _animator.SetTrigger(attack2);
            }
            else if (_comboTime < _attackTime + 0.7f && _comboTime > _attackTime && _attackIndex == 2)
            {
                _animator.SetTrigger(attack3);
            }
        }
    }

    /// <summary>
    /// Function that to reattack, _attackIndex variable has to be 0 and _comboStarted bool has to be false
    /// </summary>
    private void ResetAttack()
    {
        _comboStarted = false;
        _attackIndex = 0;
    }
}
