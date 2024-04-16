using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BasicBoneman : Summonable
{
    [SerializeField] private Transform carryPosition, grabPositionLeft, grabPositionRight;
    [SerializeField] private float grabRadius = 0.5f;
    [SerializeField] private float droppedRadius = 0.6f;
    private EnemyAI enemyAI;
    private Seeker seekerScript;

    private Transform currentTarget;
    private Entity targetGrabbedEntity;
    private Entity grabbedEntity;
    private bool hasBomb;
    private bool hasCrate;

    protected override void Start()
    {
        base.Start();

        enemyAI = GetComponent<EnemyAI>();
    }

    private void Update()
    {
        if (grabbedEntity != null && Vector2.Distance(grabbedEntity.transform.position, transform.position) > droppedRadius)
        {
            HandleCrateRelease();
        }

        if (targetGrabbedEntity && !isHolding)
        {
            MoveTowards(targetGrabbedEntity);
        }

    }

    public override void OnCommand(CommandType inputCommand, Vector2 target, Entity targetEntity)
    {
        switch (inputCommand)
        {
            case CommandType.MoveTo:
                enemyAI.target = target;
                animator.SetBool("isMoving", true);
                break;
            case CommandType.Release:
                HandleBombRelease();
                HandleCrateRelease();
                break;
            case CommandType.Grab:
                targetGrabbedEntity = targetEntity;
                break;
            case CommandType.Stack:
                // TODO: IMPLEMENT
                break;
            case CommandType.BoardCatapult:
                // TODO: IMPLEMENT
                break;
        }
    }

    private void MoveTowards(Entity targetEntity)
    {
        enemyAI.target = targetEntity.transform.position;

        if (Vector3.Distance(targetEntity.transform.position, transform.position) <= grabRadius)
        {
            HandleBombGrab(targetEntity);
            HandleCrateGrab(targetEntity);
            targetGrabbedEntity = null;
            animator.SetBool("isMoving", false);
        }
    }

    private void HandleBombGrab(Entity targetEntity)
    {
        if (targetEntity.gameObject.TryGetComponent(out Bomb bomb))
        {
            isHolding = true;
            bomb.HasBeenGrabbed();
            grabbedEntity = targetEntity;
            targetEntity.rb.bodyType = RigidbodyType2D.Kinematic;
            targetEntity.transform.position = carryPosition.position;
            targetEntity.transform.parent = carryPosition.transform;

            isHolding = true;
            hasBomb = true;
        }
    }

    private void HandleBombRelease()
    {
        if (hasBomb)
        {
            grabbedEntity.rb.bodyType = RigidbodyType2D.Dynamic;
            grabbedEntity.transform.parent = null;
            grabbedEntity.Death();

            isHolding = false;
            hasBomb = false;
        }
    }

    private void HandleCrateGrab(Entity targetEntity)
    {
        if (targetEntity.gameObject.TryGetComponent(out Crate crate))
        {
            grabbedEntity = targetEntity;
            grabbedEntity.rb.bodyType = RigidbodyType2D.Kinematic;
            if ((transform.position.x - targetEntity.gameObject.transform.position.x) < 0)
            {
                targetEntity.transform.position = grabPositionRight.position;
                targetEntity.transform.parent = grabPositionRight.transform;
            }
            else
            {
                targetEntity.transform.position = grabPositionLeft.position;
                targetEntity.transform.parent = grabPositionLeft.transform;
            }

            isHolding = true;
            hasCrate = true;
        }
    }

    private void HandleCrateRelease()
    {
        if (hasCrate)
        {
            grabbedEntity.rb.bodyType = RigidbodyType2D.Dynamic;
            grabbedEntity.transform.parent = null;
            isHolding = false;
            hasCrate = false;
        }
    }
}
