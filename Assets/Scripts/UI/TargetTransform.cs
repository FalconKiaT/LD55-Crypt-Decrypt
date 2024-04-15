using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTransform : MonoBehaviour
{
public Transform target;
    
    public void goToTarget()
    {
        Camera.main.GetComponent<CameraFollow>().setTarget(target);
    }
}
