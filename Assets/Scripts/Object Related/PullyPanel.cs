using FKTools;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PullyPanel : MonoBehaviour
{
    public PullyPanel linkedPully;

    public int currentWeight = 0;

    public float speed = 0;

    public float targetPrecision = 0.1f;

    public int initialPosition = 0;
    public int targetPosition = 0;
    public WeightedObject weightAbove;

    // Position of upper collider
    [Header("SETTINGS")]
    [SerializeField] private Transform boxCastCenter;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private LayerMask layersToCheck;

    private void Start()
    {
        initialPosition = (int)Mathf.Ceil(transform.position.y);
        targetPosition = initialPosition;
        // Start raycasting
        StartCoroutine(RayCastAtBox());
    }

    private void Update()
    {
        if (weightAbove != null)
        {
            currentWeight = weightAbove.weight;
        }
        else
        {
            currentWeight = 0;
        }
        targetPosition = initialPosition - WeightedDifference();

        if (Mathf.Abs(transform.position.y - (float)targetPosition) <= targetPrecision)
            return;
        else if (transform.position.y > (float)targetPosition)
        {
            MoveDown();
        }
        else if (transform.position.y < (float)targetPosition)
        {
            MoveUp();
        }
    }

<<<<<<< Updated upstream
    private void OnTriggerStay2D(Collider2D collision)
=======
    // Raycasting routine
    private IEnumerator RayCastAtBox()
    {
        Collider2D[] colliderArray;
        bool foundWeightOnCast = false;

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
                    weightAbove = weight;
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
            weightAbove = null;
            foundWeightOnCast = false;
            yield return FKRoutines.NullPauseAware();
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
>>>>>>> Stashed changes
    {
        if (weightAbove == null)
        {
            if (collision.gameObject.GetComponentInChildren<WeightedObject>() != null)
            {
                weightAbove = collision.gameObject.GetComponentInChildren<WeightedObject>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInChildren<WeightedObject>())
        {
            weightAbove = null;
        }
    }
    */

    private void MoveDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private int WeightedDifference()
    {
        int weightedDiff = currentWeight - linkedPully.currentWeight;
        return weightedDiff;
    }

    // Draw the raycast box
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCastCenter.position, groundCheckSize);
        Gizmos.color = Color.blue;
    }
}
