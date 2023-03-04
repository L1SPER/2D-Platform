using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHealth : Health,IDamageable<int>
{
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] HealthBar healthBar;
    private bool _untouchable = false;

    private Animator _animator;

    private void Awake()
    {
        _animator= GetComponent<Animator>();
    }
    private void Start()
    {
        _currentHealth = _maxHealth;
        healthBar.SetHealth(_currentHealth);
        healthBar.SetMaxHealth(_maxHealth);
    }
    /// <summary>
    /// Takes damage
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if(!_untouchable)
            _currentHealth -= damage;
        healthBar.SetHealth(_currentHealth);
        CheckIfWeDead();
        if(_currentHealth>0)
            _animator.SetTrigger("Hurt");
        StartCoroutine(UntouchableActive());
    }
    /// <summary>
    /// Being untouchable for 1 second
    /// </summary>
    /// <returns></returns>
    public IEnumerator UntouchableActive()
    {
        if (_untouchable)
            yield break;
        _untouchable = true;
        yield return new WaitForSeconds(1f);
        _untouchable = false;
    }

    /// <summary>
    /// Checks if enemy are dead
    /// </summary>
    public override void CheckIfWeDead()
    {
        base.CheckIfWeDead();
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _animator.SetBool("Death", true);
            FindObjectOfType<AudioManager>().Play("EnemyDie");
            Destroy(gameObject,1f);
        }
    }
}
