using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeightedObject : MonoBehaviour
{
    public int weight = 1;

    public WeightedObject WeightAbove;

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


    private void OnTriggerStay2D(Collider2D collision)
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
}
