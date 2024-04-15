using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : BalancedObject
{
    protected override void Update()
    {
        base.Update();

        targetPosition = initialPosition + currentWeight;
    }

}
