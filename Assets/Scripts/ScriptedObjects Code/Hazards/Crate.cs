using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D rb;
    public PhysicsMaterial2D slippery;
    public PhysicsMaterial2D friction;
    private BoxCollider2D boxCollider;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();   
        boxCollider.sharedMaterial = slippery;
        Debug.Log("change");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colided2");
        if (collision.gameObject.tag == "Entity")
        {

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
        }
        


    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
    /*
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            boxCollider.sharedMaterial = slippery;
            Debug.Log("colided");
        }
    }
    */
}
