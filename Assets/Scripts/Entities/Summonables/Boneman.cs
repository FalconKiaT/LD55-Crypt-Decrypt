using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boneman : Entity, ISelectable, ICommandable
{
    // Populate Dictionary of valid commands for the boneman
    private void Awake()
    {
        // Set variables

        // Populate 
    }

    // Local Variables
    private Coroutine TESTINGROUTINE = null;


    public void OnSelected()
    {
        UnitManager.instance.AddToSelected(this);
    }

    public void OnCommand(CommandType inputCommand, Vector2 target, Entity targetEntity)
    {
        switch(inputCommand)
        {
            case CommandType.MoveTo:
                MoveTo(target);
                break;
            case CommandType.ReleaseCarry:
                // TODO: IMPLEMENT
                break;
            case CommandType.Carry:
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

    /// <summary>
    /// Function that will make the skeleton move towards the target
    /// </summary>
    /// <param name="target"></param>
    private void MoveTo(Vector2 target)
    {
        Debug.Log("CALLED MOVE TO ON BONEMAN SCRIPT");
        if (TESTINGROUTINE != null)
        {
            StopCoroutine(TESTINGROUTINE);
        }
        TESTINGROUTINE = StartCoroutine(TESTINGMOVEMENT(target));
    }

    private IEnumerator TESTINGMOVEMENT(Vector2 target)
    {
        float speed = 0.1f;

        // HACK: MOVEMENT TEST
        while (Vector2.Distance((Vector2)this.transform.position, target) > 0.1f)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed);
            yield return null;
        }
    }

    
}
