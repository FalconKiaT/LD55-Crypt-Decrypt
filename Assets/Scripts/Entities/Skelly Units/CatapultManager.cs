using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CatapultManager : MonoBehaviour
{
    [Header("Physics Variables: Dynamic")]
    float velocityX;
    float velocityY;
    float totalVelocity;
    float theta;
    float height;
    float time;
    float deltaX;

    [Header("Physics Variables: Constant")]
    public float gravity = 10;
    public float heightOfParabola;

    [Header("Bezier Path Variables")]
    public GameObject pointOne;
    public GameObject pointTwo;
    public GameObject pointThree;
    public GameObject pointFour;

    [Header("Skeleton Variables")]
    public GameObject skeletonObject;
    public GameObject targetDestination;
    public bool toss;
    public bool loadCatapult;

    private void Update()
    {
        if (loadCatapult)
        {
            ParabolaMaker();
        }
    }

    public void ParabolaMaker()
    {
        // determine deltaX and deltaY
        deltaX = - targetDestination.transform.position.x + skeletonObject.transform.position.x;
        height = - targetDestination.transform.position.y + skeletonObject.transform.position.y + heightOfParabola;

        // figure out VY
        velocityY = Mathf.Sqrt(2 * gravity * height);
        time = velocityY / gravity;

        // figure out VX
        velocityX = deltaX / time;

        // figure out VTotal
        theta = Mathf.Atan2(velocityY, velocityX);
        totalVelocity = Mathf.Acos(theta) * velocityX;

        // Draws the points based on the parabola
        LineMaker();

        if (toss)
        {
            ThrowSkeleton();
            toss = false;
        }
    }

    public void LineMaker()
    {
        // start and end points
        pointOne.transform.position = skeletonObject.transform.position;
        pointFour.transform.position = targetDestination.transform.position;

        // in between points
        pointTwo.transform.position = new Vector3(deltaX / 4, height / 2, 0); 
        pointThree.transform.position = new Vector3(deltaX * 3 / 4, height / 2, 0);
    }

    public void ThrowSkeleton()
    {
        Rigidbody2D skeletonPhysics = skeletonObject.GetComponent<Rigidbody2D>();
        skeletonPhysics.velocityX = velocityX;
        skeletonPhysics.velocityY = velocityY;
    }
}
