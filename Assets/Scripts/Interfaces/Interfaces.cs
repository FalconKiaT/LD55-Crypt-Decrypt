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
    public void OnCommand(CommandType inputCommand);
}

/// <summary>
/// Any entity that can be damaged must implement this interface
/// </summary>
public interface IDamageable
{
    public void TakeDamage();
}


