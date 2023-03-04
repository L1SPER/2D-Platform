using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkEnemyCombat : MonoBehaviour
{
    [SerializeField] GameObject _firePrefab;
    [SerializeField] Transform _firePoint;
    bool _isShooting=false;
    public void Shoot()
    {
        StartCoroutine(WaitAndShoot());
    }

    IEnumerator WaitAndShoot()
    {
        if (_isShooting)
            yield break;
        _isShooting = true;
        FindObjectOfType<AudioManager>().Play("EnemyAttack");
        Instantiate(_firePrefab, _firePoint.position, _firePoint.rotation);
        yield return new WaitForSeconds(1f);
        _isShooting = false;

    }
}
