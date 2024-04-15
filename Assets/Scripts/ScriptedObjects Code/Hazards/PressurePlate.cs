using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Start is called before the first frame update
    
    public bool on = false;

    private void Start()
    {
        //originalPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided");
        if (collision.transform.tag == "Entity" || collision.transform.tag == "Object")
        {
            //collision.transform.parent = transform;
            GetComponent<SpriteRenderer>().color = Color.red;
            on = true;
            
        }
            
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Entity" || collision.transform.tag == "Object")
        {
            //collision.transform.parent = transform;
            GetComponent<SpriteRenderer>().color = Color.white;
            on = false;
        }
    }
}
