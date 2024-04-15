using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SkellyPathfinder : MonoBehaviour
{
    [Header("Important Locations")]
    public GameObject skeletonObject;
    public GameObject gapDetector;
    public GameObject jumpDetector;
    Vector3 placeToMove;

    [Header("Jumping")]
    bool canJump;
    bool jumping;
    float jumpHeight = 1.2f;

    [Header("Movement")]
    Animator skeletonAnimator;
    bool canMove = true;
    int collisionCounter = 0;

    public bool startMoving;
    float speed;

    // Update is called once per frame
    void Update()
    {
        // changes collider position
        transform.position = skeletonObject.transform.position + new Vector3(0, 0, 0);
        gapDetector.transform.position = skeletonObject.transform.position + new Vector3(0, -1, 0);

        // get animator
        skeletonAnimator = skeletonObject.GetComponent<Animator>();

        if (startMoving)
        {
            CheckDirection();
            MoveToPlace();
        }
        else
        {
            skeletonAnimator.SetBool("isMoving", false);
        }
    }
    public void CheckDirection() {
        CollisionChecker();

        // figures out delta x
        if (placeToMove.x > skeletonObject.transform.position.x)
        {
            speed = 0.02f;
        }
        else 
        {
            speed = -0.02f;
        }

        // if the skelly encounters a wall
        if (!canMove && jumpDetector.GetComponent<SkellyCollisionHandler>().getMove() && !jumping)
        {
            canMove = true;
            canJump = true;
        }
        else if (!canMove && !jumping)
        {
            startMoving = false;
        }
    }

    public void MoveToPlace() {
        // determines if the skelly has arrived
        float range = 0.1f;

        // if the skelly is approaching from the left
        if (speed > 0)
        {
            if (skeletonObject.transform.position.x > placeToMove.x - range)
            {
                startMoving = false;
                canMove = false;
            }
        }
        // if the skelly is approaching from the right 
        else
        {
            if (skeletonObject.transform.position.x < placeToMove.x + range)
            {
                startMoving = false;
                canMove = false;
            }
        }

        if (canMove)
        {
            // starts the animator
            skeletonAnimator.SetFloat("directionX", -speed);
            skeletonAnimator.SetBool("isMoving", true);

            // determines what the skeleton is doing
            if (canJump && !jumping)
            {
                skeletonObject.transform.Translate(new Vector3(speed*4f, jumpHeight, 0)); 
                jumping = true;
                canJump = false;
            }
            else
            {
                skeletonObject.transform.Translate(new Vector3(speed, 0, 0));
            }

            // stops the jump
            if (!gapDetector.GetComponent<SkellyCollisionHandler>().getMove())
            {
                jumping = false;
            }
        }
    }

    private void CollisionChecker()
    {
        if (collisionCounter == 0)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }

    public void StartPathfinding(Vector3 targetPosition)
    {
        startMoving = true;
        canMove = true;
        placeToMove = targetPosition;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // collision counter is number of objects colliding with the current object
        if (collision != null)
        {
            collisionCounter++;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            collisionCounter--;
        }
    }
}
