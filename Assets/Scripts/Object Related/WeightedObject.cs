using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FKTools;

public class WeightedObject : MonoBehaviour
{
    public int weight = 1;

    public WeightedObject WeightAbove;

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

    private void Update()
    {
        if (WeightAbove != null)
        {
            weight = WeightAbove.weight + 1;
        }
        else
        {
            weight = 1;
        }
    }

    // Continuosly check aboce
    private IEnumerator RayCastAtBox()
    {
        Collider2D[] colliderArray;
        bool foundWeightOnCast = false;

<<<<<<< Updated upstream
    private void OnTriggerStay2D(Collider2D collision)
=======
        while (true)
        {
            colliderArray = Physics2D.OverlapBoxAll(boxCastCenter.position, groundCheckSize, 0, layersToCheck);
            // Check the result list
            foreach (Collider2D currCollider in colliderArray)
            {
                // Check if we found any objects that implement weighted object
                if (currCollider.gameObject.TryGetComponent(out WeightedObject weight))
                {
                    // Check that its not this object
                    if (weight == this) continue;

                    // We did find one
                    WeightAbove = weight;
                    foundWeightOnCast = true;
                    break;
                }
            }
            // if we did find one, repeat loop
            if (foundWeightOnCast)
            {
                foundWeightOnCast = false;
                yield return FKRoutines.NullPauseAware();
                continue;
            }
            // else, we did not find one
            WeightAbove = null;
            foundWeightOnCast = false;
            yield return FKRoutines.NullPauseAware();
        }
    }


    /*
    private void OnTriggerEnter2D(Collider2D collision)
>>>>>>> Stashed changes
    {
        if (WeightAbove == null)
        {
            if(collision.gameObject.TryGetComponent(out WeightedObject weight))
            {
                WeightAbove = weight;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out WeightedObject weight))
        {
            WeightAbove = null;
        }
    }
    */

    // Draw the raycast box
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCastCenter.position, groundCheckSize);
        Gizmos.color = Color.blue;
    }
}
