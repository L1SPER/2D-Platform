using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] public int _maxHealth;
    [SerializeField] public int _currentHealth;
    public HealthBar healthBar;
   
    private void Start()
    {
        _currentHealth = _maxHealth;
        healthBar.SetMaxHealth(_maxHealth);
        healthBar.SetHealth(_currentHealth);
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        _currentHealth -= damage;
        healthBar.SetHealth(_currentHealth);
        if (_currentHealth < 0)
        {
            _currentHealth = 0;
            Die();
        }
    }
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
