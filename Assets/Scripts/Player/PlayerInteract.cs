using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private KeyCode _interactionKey = KeyCode.E;
    private float _interactionRange = 2f;
    private void Update()
    {
        if(Input.GetKeyDown(_interactionKey))
        {
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, _interactionRange);
            foreach(Collider2D collider in colliderArray)
            {
                if(collider.TryGetComponent(out InteractableObject interactableObject))
                {

                }
            }
        }
    }
}
