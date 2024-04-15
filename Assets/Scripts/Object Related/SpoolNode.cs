using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoolNode : MonoBehaviour
{
    public PressurePlate linkedPressurePlate;
    public BalancedObject balancedObject;

    public int weightAppliedWhenActive = 0;


    // Update is called once per frame
    void Update()
    {
        if (balancedObject == null || linkedPressurePlate == null)
            return;

        if (linkedPressurePlate.on)
        {
            balancedObject.currentWeight = weightAppliedWhenActive;
        }
        else
        {
            balancedObject.currentWeight = 0;
        }
    }
}
