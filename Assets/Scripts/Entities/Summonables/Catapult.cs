using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Entity, ISelectable, ICommandable
{
    private void Awake()
    {

    }

    public void OnSelected()
    {
        UnitManager.instance.AddToSelected(this);
    }

    public void OnCommand(CommandType inputCommand, Vector2 target, Entity targetEntity)
    {

    }
}
