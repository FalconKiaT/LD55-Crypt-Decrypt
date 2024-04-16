using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredBoneman : Summonable
{
    private EnemyAI enemyAI;

    protected override void Start()
    {
        base.Start();

        enemyAI = GetComponent<EnemyAI>();
    }

    public override void OnCommand(CommandType inputCommand, Vector2 target, Entity targetEntity)
    {
        switch (inputCommand)
        {
            case CommandType.MoveTo:
                enemyAI.target = target;
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
