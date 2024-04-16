using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) 
        {
            gameObject.GetComponent<PlayerStats>().Death();
        }
    }
}
