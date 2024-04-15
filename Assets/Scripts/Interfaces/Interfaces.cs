using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any entity that interacts with the selection system must use this interface
/// </summary>
public interface ISelectable
{
    public void OnSelected();
}

public interface ICommandable
{
    // The target vector will evaluate to the world point of the mouse
    // The target entity will be the entity at the mouse position
    public void OnCommand(CommandType inputCommand, Vector2 target, Entity targetEntity);
}

/// <summary>
/// Any entity that can be damaged must implement this interface
/// </summary>
public interface IDamageable
{
    public void TakeDamage(int damage);
    public void Death();
}


