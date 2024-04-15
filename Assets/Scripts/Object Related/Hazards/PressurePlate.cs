using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool on = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        bool condition = collision.transform.tag == "Entity" || collision.transform.tag == "Player" || collision.transform.tag == "Objects";
        if (condition)
        {
            on = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        bool condition = collision.transform.tag == "Entity" || collision.transform.tag == "Player" || collision.transform.tag == "Objects";
        if (condition) 
        {
            on = false;
        }
    }
}
