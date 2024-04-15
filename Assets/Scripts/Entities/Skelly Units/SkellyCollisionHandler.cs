using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkellyCollisionHandler : MonoBehaviour
{
    private int canMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision!=null)
        {
            canMove++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision!=null)
        {
            canMove--;
        }
    }

    public bool getMove()
    {
        if (canMove == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
