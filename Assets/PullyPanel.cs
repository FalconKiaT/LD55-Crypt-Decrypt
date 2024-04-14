using System.Collections;
using Unity.Android.Types;
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
    private WeightedObject weightAbove;

    private void Start()
    {
        initialPosition = (int)Mathf.Ceil(transform.position.y);
        targetPosition = initialPosition;
    }

    private void Update()
    {
        Debug.Log(WeightedDifference());
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

    private void OnTriggerEnter2D(Collider2D collision)
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
        Debug.Log(this.name + " " + weightedDiff);
        return weightedDiff;
    }
}
