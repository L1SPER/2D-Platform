using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject _player;
    void LateUpdate()
    { 
        if(_player)
            this.transform.position=_player.transform.position;
    }
}
