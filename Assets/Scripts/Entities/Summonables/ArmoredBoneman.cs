using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredBoneman : Summonable
{
    public override void OnCommand(CommandType inputCommand, Vector2 target, Entity targetEntity)
    {
        switch (inputCommand)
        {
            case CommandType.MoveTo:
                MoveTo(target);
                break;
            case CommandType.Release:
                // TODO: IMPLEMENT
                break;
            case CommandType.Grab:
                // TODO: IMPLEMENT
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
