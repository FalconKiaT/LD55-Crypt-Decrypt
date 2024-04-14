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
    bool canMove = true;

    public bool startMoving;

    bool canJump;
    bool canDrop;
    float speed = 0.1f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startMoving)
        {
            CheckDirection();
            MoveToPlace();
        }
    }
    public IEnumerator CheckDirection() {
        canMove = true;
        float xDirection;

        // figures out delta x
        if (placeToMove.transform.position.x > skeletonObject.transform.position.x)
        {
            speed = 0.01f;
            xDirection = 1f;
        }
        else 
        {
            speed = -0.01f;
            xDirection = -1f;
        }

        // changes collider position
        transform.position = skeletonObject.transform.position + new Vector3(0, 0, 0);
        gapDetector.transform.position = skeletonObject.transform.position + new Vector3(0, -1, 0);

        // if the skelly encounters a wall
        if (!canMove && gapDetector.GetComponent<SkellyCollisionHandler>().canMove)
        {
            canMove = true;
            transform.position = skeletonObject.transform.position +  new Vector3(0, 1, 0);
            yield return new WaitForEndOfFrame();
            canJump = false;
            if (canMove)
            {
                canJump = true;
            }
        }
        
        // if the skelly encounters a gap
        if (!gapDetector.GetComponent<SkellyCollisionHandler>().canMove && canMove) 
        {
            gapDetector.transform.position = skeletonObject.transform.position + new Vector3(xDirection, -2, 0);
            // if there is a block under
            if (gapDetector.GetComponent<SkellyCollisionHandler>().canMove)
            {
                canMove = true;
                canDrop = true;
            }
            else
            {
                canMove = false;
                canDrop = false;
            }
        }
    }

    public void MoveToPlace() { 
        
        if (canMove)
        {
            if (canJump)
            {
                skeletonObject.transform.Translate(new Vector3(speed/2, 0.001f, 0));
            }
            else if (canDrop)
            {
                skeletonObject.transform.Translate(new Vector3(speed, 0, 0));
            }
            else
            {
                skeletonObject.transform.Translate(new Vector3(speed, 0, 0));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            canMove = false;
            canJump = false;
            if (collision.name == "PlaceToMove")
            {
                startMoving = false;
            }
        }
        
    }
}
