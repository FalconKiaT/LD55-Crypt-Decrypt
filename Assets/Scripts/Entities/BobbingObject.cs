using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BobbingObject : MonoBehaviour
{
    public bool canBob = true;

    public float bobSpeed = 1f; // Speed of the bobbing motion
    public float bobDistance = 1f; // Distance the object will bob up and down

    private Vector3 startPos;
    private float startTime;

    void Start()
    {
        startPos = transform.position;
        startTime = Time.time;
    }

    void Update()
    {
        if (!canBob)
        {
            return;
        }

        // Calculate the vertical offset based on time and speed
        float yOffset = Mathf.Sin((Time.time - startTime) * bobSpeed) * bobDistance;

        // Update the object's position based on the calculated offset
        transform.position = startPos + new Vector3(0f, yOffset, 0f);
    }
}
