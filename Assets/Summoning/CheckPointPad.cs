using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointPad : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (InputManager.isInteracting && collision.tag == "Player")
        {
            EventData.RaiseOnCheckpoint();
            Camera.main.GetComponent<CameraFollow>().resetCamera();
        }
    }
}
