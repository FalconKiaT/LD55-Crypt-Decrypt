using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBoneman : Summonable
{
    [SerializeField] private Transform carryPosition;
    [SerializeField] private Entity grabbedEntity;

    public override void OnCommand(CommandType inputCommand, Vector2 target, Entity targetEntity)
    {
        switch (inputCommand)
        {
            case CommandType.MoveTo:
                MoveTo(target);
                break;
            case CommandType.Release:
                
                isHolding = false;
                HandleBombRelease();
                break;
            case CommandType.Grab:
                isHolding = true;
                HandleBombGrab(targetEntity);
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
        grabbedEntity = targetEntity;
        targetEntity.rb.bodyType = RigidbodyType2D.Kinematic;
        targetEntity.transform.position = carryPosition.position;
        targetEntity.transform.parent = carryPosition.transform;
    }

    private void HandleBombRelease()
    {
        grabbedEntity.rb.bodyType = RigidbodyType2D.Dynamic;
        grabbedEntity.transform.parent = null;
        grabbedEntity.Death();
    }
}
