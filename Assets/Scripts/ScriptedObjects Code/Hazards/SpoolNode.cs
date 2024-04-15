using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoolNode : MonoBehaviour
{
    public PressurePlate linkedPressurePlate;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(linkedPressurePlate.on);
    }

    // Update is called once per frame
    void Update()
    {
       
        if (linkedPressurePlate.on == true)
        {
            Debug.Log(linkedPressurePlate.on);
        }
        else{
            Debug.Log(linkedPressurePlate.on);
        }
    }
}
