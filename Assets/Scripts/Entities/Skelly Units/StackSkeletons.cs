using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSkeletons : MonoBehaviour
{
    bool stack;
    public bool move;
    public GameObject skeletonBottom;
    GameObject skeletonTop;

    private void Start()
    {
        skeletonTop = this.gameObject;
    }

    void Update()
    {
        if (move)
        {
            // moves skeleton2 to skeleton1
            skeletonTop.GetComponentInChildren<EntityPathfinder>().StartPathfinding(skeletonBottom.transform.position);

            move = false;
            stack = true;
        }

        if (!skeletonTop.GetComponentInChildren<EntityPathfinder>().startMoving && stack)
        {
            Stack();
        }
    }

    private void Stack()
    {
        // stacks the skellies
        skeletonBottom.GetComponent<Animator>().SetBool("isMoving", false);
        skeletonBottom.GetComponent<Animator>().SetBool("isHolding", true);
        skeletonTop.GetComponent<Animator>().SetBool("isMoving", false);

        skeletonTop.transform.position = skeletonBottom.transform.position + new Vector3(0f, 1f, 0f);

        // ensures skeleton 1 can't move
        skeletonBottom.GetComponentInChildren<EntityPathfinder>().startMoving = false;
    }

    // newStack determines if the skeletons will start stacking and newskeleton2 is the skeleton to stack on
    public void SetStack(bool newStack, GameObject newSkeleton2)
    {
        if (skeletonBottom.transform.position != newSkeleton2.transform.position)
        {
            // starts the stack
            move = newStack;
            skeletonTop = newSkeleton2;
        }
    }
}
