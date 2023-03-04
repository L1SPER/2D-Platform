using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemyCombat : MonoBehaviour
{
    [SerializeField] private int _attackDamage = 1;
    [SerializeField] private float _attackRange;
    [SerializeField] GameObject _attackPoint;
    [SerializeField] LayerMask _targetLayer;
    private bool _isAttacking = false;
    private void CheckHit()
    {
        Collider2D[] hitResults = Physics2D.OverlapCircleAll(_attackPoint.transform.position, _attackRange, _targetLayer);
        if (hitResults == null)
            return;

        foreach (Collider2D hit in hitResults)
        {
            Debug.Log(hit.name);
            if(hit.GetComponent<IDamageable<int>>()!= null)
            {
                hit.GetComponent<IDamageable<int>>().TakeDamage(_attackDamage);
            }
        }
    }
    public  void StartAttack()
    {
        StartCoroutine(WaitAttack());
        if(_isAttacking)
            CheckHit();
    }
    IEnumerator WaitAttack()
    {
        if (_isAttacking)
            yield break;
        FindObjectOfType<AudioManager>().Play("EnemyAttack");
        _isAttacking = true;
        yield return new WaitForSeconds(1f);
        _isAttacking= false;
    }
}
