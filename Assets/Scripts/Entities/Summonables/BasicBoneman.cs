using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBoneman : Summonable
{
    public override void OnCommand(CommandType inputCommand, Vector2 target, Entity targetEntity)
    {
        switch (inputCommand)
        {
            case CommandType.MoveTo:
                entityPathfinder.StartPathfinding(target);
                break;
            case CommandType.Release:
                // TODO: IMPLEMENT
                isHolding = false; // REMOVE THIS AND HANDLE WITH MOVEMENT
                break;
            case CommandType.Grab:
                // TODO: IMPLEMENT
                isHolding = true; // REMOVE THIS AND HANDLE WITH MOVEMENT
                break;
            case CommandType.Stack:
                // TODO: IMPLEMENT
                break;
            case CommandType.BoardCatapult:
                // TODO: IMPLEMENT
                break;
        }
    }
}
