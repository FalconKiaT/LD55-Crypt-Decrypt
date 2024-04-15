using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponentInChildren<Summon>() != null)
        {
            collision.GetComponentInChildren<Summon>().AddOneBone();
            Destroy(gameObject, 0.1f) ;
        }    
    }
}
