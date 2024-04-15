using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonBoardcatapult : MonoBehaviour
{
    bool board;
    public bool move;
    public bool swing;
    public GameObject catapultArmObject;
    Vector3 target;
    GameObject skeletonTop;

    private void Start()
    {
        skeletonTop = this.gameObject;
    }

    void Update()
    {
        // sets the skeleton's target as mouse position
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        // boundary maker
        int direction = catapultArmObject.GetComponentInParent<CatapultManager>().direction;
        if (!(worldPosition.x < - direction * 50 + transform.position.x ) && !(worldPosition.x > transform.position.x) &&
            !(worldPosition.y > transform.position.y) && !(worldPosition.y < transform.position.y + 50))
        {
            target = worldPosition;
            catapultArmObject.GetComponentInParent<CatapultManager>().SetSkeleton(this.gameObject, target);
        }

        if (move)
        {
            // moves skeleton to catapult
            skeletonTop.GetComponentInChildren<EntityPathfinder>().StartPathfinding(catapultArmObject.transform.position);

            // animations
            catapultArmObject.GetComponent<Animator>().SetBool("Swing", false);
            catapultArmObject.GetComponent<Animator>().SetBool("Reload", true);

            // loads the skeleton
            catapultArmObject.GetComponentInParent<CatapultManager>().loadCatapult = true;

            move = false;
            board = true;
        }

        if (!skeletonTop.GetComponentInChildren<EntityPathfinder>().startMoving && board)
        {
            if (Input.GetMouseButtonDown(1)) { swing = true; }
            if (Input.GetMouseButtonDown(2)) { board = false; }

            if (swing)
            {
                catapultArmObject.GetComponent<Animator>().SetBool("Reload", false);
                catapultArmObject.GetComponent<Animator>().SetBool("Swing", true);

                catapultArmObject.GetComponentInParent<CatapultManager>().toss = true;

                swing = false;
                board = false;
            }
            else
            {
                Board();
            }
        }

    }

    private void Board()
    {
        // stacks the skellies
        skeletonTop.GetComponent<Animator>().SetBool("isMoving", false);

        skeletonTop.transform.position = catapultArmObject.transform.position + new Vector3(1f, 0f, 0f);
    }

    // newBoard determines if the skeletons will start boarding and newArm is the catapult to board
    public void SetBoard(bool newBoard, GameObject newArm)
    {
        if (catapultArmObject.transform.position != newArm.transform.position)
        {
            // starts the boarding
            move = newBoard;
            skeletonTop = newArm;
        }
    }
}
