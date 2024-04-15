using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Paladin : Enemy
{
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float pointPrecision = 0.01f;

    public int patrolIndex = 0;
    public Vector2 moveDirection;
    public Transform currentPatrolPoint;

    protected override void Start()
    {
        base.Start();
        currentPatrolPoint = patrolPoints[patrolIndex];
    }

    private void FixedUpdate()
    {
        if (patrolPoints.Count <= 0)
            return;

        if (!isInCombat)
        {
            MoveToNextPatrolPoint();
            CheckPosition();
            animator.SetBool("isMoving", true);
        }
        else
            animator.SetBool("isMoving", false);
    }

    private void MoveToNextPatrolPoint()    // Move to the next patrol point in the list constantly
    {
        moveDirection = patrolPoints[patrolIndex].position - transform.position;
        moveDirection.Normalize();
        
        animator.SetFloat("directionX", Mathf.Sign(moveDirection.x));

        rb.MovePosition((Vector2)transform.position + (moveDirection * Time.deltaTime * movementSpeed));
    }

    private void CheckPosition()    // Check the distance from the enemy and target position and switch patrol points if close
    {
        float difference = transform.position.x - currentPatrolPoint.position.x;

        if (Mathf.Abs(difference) < pointPrecision)
        {
            patrolIndex++;
            if (patrolIndex >= patrolPoints.Count)
            {
                patrolIndex = 0;
            }

            currentPatrolPoint = patrolPoints[patrolIndex];
        }
    }
}
