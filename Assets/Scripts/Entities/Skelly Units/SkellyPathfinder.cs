using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SkellyPathfinder : MonoBehaviour
{
    public GameObject skeletonObject;
    public GameObject placeToMove;
    public GameObject gapDetector;
    public GameObject jumpDetector;
    Animator skeletonAnimator;
    bool canMove = true;
    int collisionCounter = 0;

    public bool startMoving;

    bool canJump;
    bool jumping;
    float speed;
    

    // Start is called before the first frame update
    void Start()
    {
        // changes collider position
        transform.position = skeletonObject.transform.position + new Vector3(0, 0, 0);
        gapDetector.transform.position = skeletonObject.transform.position + new Vector3(0, -1, 0);

        // get animator
        skeletonAnimator = skeletonObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startMoving)
        {
            CheckDirection();
            AnimateSkeleton();
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
        if (placeToMove.transform.position.x > skeletonObject.transform.position.x)
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
        if (canMove)
        {
            // starts the animator
            skeletonAnimator.SetFloat("directionX", -speed);
            skeletonAnimator.SetBool("isMoving", true);

            // determines what the skeleton is doing
            if (canJump && !jumping)
            {
                skeletonObject.transform.Translate(new Vector3(speed*4f, 1.2f, 0)); 
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

    public void AnimateSkeleton()
    {
        
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            collisionCounter++;
            if (collision.name == "PlaceToMove")
            {
                startMoving = false;
            }
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
