using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (collision.gameObject.tag == "Entity")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;  
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Entity")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    
}
