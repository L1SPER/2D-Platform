using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health,IDamageable<int>
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] HealthBar healthBar;
   
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
        _currentHealth -= damage;
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
            _currentHealth = 0;
            Destroy(gameObject);
        }
    }
}
