using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointPad : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (InputManager.isInteracting && collision.tag == "Player")
        {
            EventData.NukeBonemen();
            Camera.main.GetComponent<CameraFollow>().resetCamera();
            ScenesManager.instance.CheckpointReached();
        }
    }
}
