using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeightedObject : MonoBehaviour
{

    public int weight = 1;
    public bool touching;

    private WeightedObject WeightAbove;

    public int touchedpanel = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

  

    public int getWeight()
    {
        return weight;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name);
        if(WeightAbove == null)
        {
            if(collision.gameObject.GetComponent<WeightedObject>() != null)
            {

                Debug.Log("TEST");
               WeightAbove = collision.gameObject.GetComponent<WeightedObject>();
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<WeightedObject>() != null)
        {
            WeightAbove = null;
        }
    }
    
    private void Update()
    {
        if(WeightAbove != null)
        {
            weight = WeightAbove.weight +1;
        }
        else
        {
            weight=0;
        }
    }
}
