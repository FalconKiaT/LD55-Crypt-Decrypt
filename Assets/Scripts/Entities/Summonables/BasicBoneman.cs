using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBoneman : Summonable
{
    [SerializeField] private Transform carryPosition;

    public override void OnCommand(CommandType inputCommand, Vector2 target, Entity targetEntity)
    {
        switch (inputCommand)
        {
            case CommandType.MoveTo:
                MoveTo(target);
                break;
            case CommandType.Release:
                
                isHolding = false;
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
        targetEntity.transform.position = carryPosition.position;
        targetEntity.transform.parent = carryPosition.transform;
    }
}
