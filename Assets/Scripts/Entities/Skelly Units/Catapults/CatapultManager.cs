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
    float time;
    float deltaX;

    [Header("Physics Variables: Constant")]
    public float gravity = 10;
    public float height;

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
    public bool rotate;

    private void Update()
    {
        if (rotate)
        {
            if (transform.rotation.y == 1)
            {
                transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            }
            else
            {
                transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            }
            rotate = false;
        }
        if (loadCatapult)
        {
            ParabolaMaker();
        }
    }

    public void ParabolaMaker()
    {
        // determine deltaX and deltaY
        deltaX = targetDestination.transform.position.x - skeletonObject.transform.position.x;

        // figure out VY
        velocityY = Mathf.Sqrt(2 * gravity * height);
        time = velocityY / gravity;

        // figure out VX
        velocityX = deltaX / (2 * time);

        // figure out VTotal
        theta = Mathf.Atan2(velocityY, velocityX);
        totalVelocity = velocityX / Mathf.Cos(theta); 

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
        if (transform.rotation.y == 1 || transform.rotation.y == -1)
        {
            pointThree.transform.position = new Vector3(skeletonObject.transform.position.x + totalVelocity / 2, skeletonObject.transform.position.y + height / 2, 0);
            pointTwo.transform.position = new Vector3(targetDestination.transform.position.x - totalVelocity / 2, skeletonObject.transform.position.y + height / 2, 0);
        }
        if (transform.rotation.y == 0)
        {
            pointThree.transform.position = new Vector3(skeletonObject.transform.position.x - totalVelocity / 2, skeletonObject.transform.position.y + height / 2, 0);
            pointTwo.transform.position = new Vector3(targetDestination.transform.position.x + totalVelocity / 2, skeletonObject.transform.position.y + height / 2, 0);
        }
        
    }

    public void ThrowSkeleton()
    {
        Rigidbody2D skeletonPhysics = skeletonObject.GetComponent<Rigidbody2D>();
        skeletonPhysics.velocityX = velocityX;
        skeletonPhysics.velocityY = velocityY;
        loadCatapult = false;
    }
}
