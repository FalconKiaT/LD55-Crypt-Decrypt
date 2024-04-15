using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FKTools;

public enum EntityTypes
{
    NULL,
    Boneman,
    Catapult,
    ArmoredBoneman,
    Bomb,
    Paladin,
    Player,
    Crate
}

public abstract class Entity : FKMonoBehaviour, ISelectable, ICommandable, IDamageable
{
    public EntityTypes entityType;

    public int health = 0;
    public int damage = 0;

    public virtual void TakeDamage(int damage)
    {
        if (damage >= health)
        {
            Death();
        }
        else
            health -= damage;
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public virtual void OnSelected() { }

    public virtual void OnCommand(CommandType inputCommand, Vector2 target, Entity targetEntity) { }
}
