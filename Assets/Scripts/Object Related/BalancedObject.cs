using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancedObject : MonoBehaviour
{
    public int currentWeight = 0;

    public float speed = 0;

    public float targetPrecision = 0.1f;

    public int initialPosition = 0;
    public int targetPosition = 0;

    private void Start()
    {
        initialPosition = (int)Mathf.Ceil(transform.position.y);
        targetPosition = initialPosition;
    }

    protected virtual void Update()
    {
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

    private void MoveDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
