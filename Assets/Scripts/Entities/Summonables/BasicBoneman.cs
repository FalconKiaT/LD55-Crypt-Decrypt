using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBoneman : Summonable
{
    [SerializeField] private Transform carryPosition, grabPositionLeft, grabPositionRight;
    [SerializeField] private Entity grabbedEntity;
    [SerializeField] private float droppedRadius = 0.5f;
    private GameObject dummyTransform;
    private EnemyAI enemyAI;

    private bool hasBomb;
    private bool hasCrate;

    protected override void Start()
    {
        base.Start();

        enemyAI = GetComponent<EnemyAI>();
    }

    private void Update()
    {
        // TODO: Handle crate release if out of a radius

    }

    public override void OnCommand(CommandType inputCommand, Vector2 target, Entity targetEntity)
    {
        switch (inputCommand)
        {
            case CommandType.MoveTo:
                enemyAI.target = target;
                break;
            case CommandType.Release:
                isHolding = false;
                HandleBombRelease();
                HandleCrateRelease();
                break;
            case CommandType.Grab:
                isHolding = true;
                HandleBombGrab(targetEntity);
                HandleCrateGrab(targetEntity);
                break;
            case CommandType.Stack:
                // TODO: IMPLEMENT
                break;
            case CommandType.BoardCatapult:
                // TODO: IMPLEMENT
                break;
        }
    }

    private void HandleBombGrab(Entity targetEntity)
    {
        if (targetEntity.gameObject.TryGetComponent(out Bomb bomb))
        {
            bomb.HasBeenGrabbed();
            grabbedEntity = targetEntity;
            targetEntity.rb.bodyType = RigidbodyType2D.Kinematic;
            targetEntity.transform.position = carryPosition.position;
            targetEntity.transform.parent = carryPosition.transform;

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

            hasBomb = false;
        }
    }

    private void HandleCrateGrab(Entity targetEntity)
    {
        if (targetEntity.gameObject.TryGetComponent(out Crate crate))
        {
            grabbedEntity = targetEntity;
            grabbedEntity.rb.bodyType = RigidbodyType2D.Kinematic;
            if ((transform.position.x - targetEntity.gameObject.transform.position.x) > 0)
            {
                targetEntity.transform.position = grabPositionRight.position;
                targetEntity.transform.parent = grabPositionRight.transform;
            }
            else
            {
                targetEntity.transform.position = grabPositionLeft.position;
                targetEntity.transform.parent = grabPositionLeft.transform;
            }

            hasCrate = true;
        }
    }

    private void HandleCrateRelease()
    {
        if (hasCrate)
        {
            grabbedEntity.rb.bodyType = RigidbodyType2D.Dynamic;
            grabbedEntity.transform.parent = null;
            hasCrate = false;
        }
    }
}
