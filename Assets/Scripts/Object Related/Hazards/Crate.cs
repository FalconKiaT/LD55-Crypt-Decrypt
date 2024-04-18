using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Entity
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    public override void Death()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Entity")
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
        if (collision.gameObject.tag == "Entity")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    
}
