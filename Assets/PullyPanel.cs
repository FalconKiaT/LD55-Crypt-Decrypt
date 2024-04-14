using System.Collections;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UIElements;

public class PullyPanel : MonoBehaviour
{
    public int currentWeight = 0;

    public float speed = 0;

    public WeightedObject weightAbove;

    public float initialPosition = 0;
    public float currentPosition = 0;
    public float targetPosition = 0;

    private float tileHeight = 1;
    private bool canMoveDown;

    private void Start()
    {
        initialPosition = transform.position.y;
        //targetPosition = initialPosition;
    }

    private void Update()
    {
        if (weightAbove != null)
        {
            currentWeight = weightAbove.weight;
            targetPosition = initialPosition - currentWeight * tileHeight;
        }
        else
        {
            currentWeight = 0;
            targetPosition = initialPosition;
        }

        if ()
            MoveDown();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name + " has entered");
        if (weightAbove == null)
        {
            if (collision.gameObject.GetComponentInChildren<WeightedObject>() != null)
            {
                weightAbove = collision.gameObject.GetComponentInChildren<WeightedObject>();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name + " has exited");
        if (collision.gameObject.GetComponentInChildren<WeightedObject>())
        {
            weightAbove = null;
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * Time.deltaTime);
    }
}
