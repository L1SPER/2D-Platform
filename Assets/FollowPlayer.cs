using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject Player;
    void LateUpdate()
    {
        this.transform.position=Player.transform.position;
    }
}
