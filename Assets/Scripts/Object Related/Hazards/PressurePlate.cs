using FKTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool on = false;

    // Position of upper collider
    [Header("SETTINGS")]
    [SerializeField] private Transform boxCastCenter;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private LayerMask layersToCheck;

    // Start coroutine
    private void Start()
    {
        StartCoroutine(RayCastAtBox());
    }

    private IEnumerator RayCastAtBox()
    {
        while (true)
        {
            bool foundEntity = false;
            Collider2D[] colliderArray;
            colliderArray = Physics2D.OverlapBoxAll(boxCastCenter.position, groundCheckSize, 0, layersToCheck);
            // Check the result list
            foreach (Collider2D currCollider in colliderArray)
            {
                // Check if we found any objects that implement weighted object
                if (currCollider.gameObject.TryGetComponent(out Entity entity))
                {
                    foundEntity = true;
                    break;
                }
            }
            if (foundEntity)
                on = true;
            else
                on = false;
            yield return null;
        }
    }

    // Draw the raycast box
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCastCenter.position, groundCheckSize);
        Gizmos.color = Color.blue;
    }
}
