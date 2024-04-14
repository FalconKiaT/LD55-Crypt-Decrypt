using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkellyCollisionHandler : MonoBehaviour
{
    public bool canMove = false;

    private void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            canMove = true;
        }
    }
}
