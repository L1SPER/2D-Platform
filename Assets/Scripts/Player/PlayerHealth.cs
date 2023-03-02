using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health,IDamageable<int>
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] HealthBar healthBar;
    private Animator _animator;
    private bool _untouchable = false;
    private void Awake()
    {
        _animator= GetComponent<Animator>();
    }
    private void Start()
    {
        _currentHealth = _maxHealth;
        healthBar.SetMaxHealth(_maxHealth);
        healthBar.SetHealth(_currentHealth);
    }
    /// <summary>
    /// Takes damage
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (!_untouchable)
            _currentHealth -= damage;
        if(_currentHealth>0)
            _animator.SetTrigger("Hurt");
        StartCoroutine(UntouchableActive());
        healthBar.SetHealth(_currentHealth);
        CheckIfWeDead();
    }
    /// <summary>
    /// Checks if player is dead
    /// </summary>
    public override void CheckIfWeDead()
    {
        base.CheckIfWeDead();
        if (_currentHealth <= 0)
        {
            _animator.SetBool("Death",true);
            _currentHealth = 0;
            Destroy(gameObject,1f);
        }
    }
    IEnumerator UntouchableActive()
    {
        if (_untouchable)
            yield break;
        _untouchable = true;
        yield return new WaitForSeconds(1f);
        _untouchable = false;
    }
}
