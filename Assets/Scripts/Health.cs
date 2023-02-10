using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public virtual void CheckIfWeDead() 
    {
        Debug.Log("Checking if we are dead");
    }
}
