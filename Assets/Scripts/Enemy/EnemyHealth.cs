using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHealth : Health,IDamageable<int>
{
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] HealthBar healthBar;
    
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
        _currentHealth -= damage;
        healthBar.SetHealth(_currentHealth);
        CheckIfWeDead();
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
            Destroy(gameObject);
        }
    }
}
