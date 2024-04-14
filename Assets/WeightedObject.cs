using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeightedObject : MonoBehaviour
{
    public int weight = 1;
    public bool touching;

    public WeightedObject WeightAbove;

    public int touchedpanel = 100;

    private void Update()
    {
        if (WeightAbove != null)
        {
            weight = WeightAbove.weight + 1;
        }
        else
        {
            weight = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name + " has collided");
        if (WeightAbove == null)
        {
            if(collision.gameObject.GetComponentInChildren<WeightedObject>() != null)
            {
               Debug.Log(collision.gameObject.name + " is here");
               WeightAbove = collision.gameObject.GetComponentInChildren<WeightedObject>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInChildren<WeightedObject>() != null)
        {
            WeightAbove = null;
        }
    }
}
