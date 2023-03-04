using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Fire : MonoBehaviour
{
    [SerializeField] GameObject _player;
    PinkEnemyController pinkEnemyController;
    Rigidbody2D _rb;
    private float _moveSpeed=10f;
    private void Awake()
    {
        _rb= GetComponent<Rigidbody2D>();
        pinkEnemyController= FindObjectOfType<PinkEnemyController>();
    }
    private void Update()
    {
        Movement();
    }
    private void Movement()
    {
        if(pinkEnemyController._lookingRight)
        {
            _rb.velocity = new Vector2(_moveSpeed, 0f);
        }
        else
        {
            _rb.velocity=new Vector2(-_moveSpeed, 0f);
        }
    }
}
