using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FKTools;

public enum EntityTypes
{
    NULL,
    BasicBoneman,
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

    [SerializeField] private float deathDelay = 0;

    public bool canFight;

    protected Animator animator;
    protected Rigidbody2D rb;

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
        StartCoroutine(AttackCo());
    }

    public void AttackAnim(Vector2 attackDirection)
    {
        animator.SetFloat("directionX", Mathf.Sign(attackDirection.x));
        animator.SetTrigger("attackTrigger");
    }

    private IEnumerator AttackCo()
    {
        yield return FKRoutines.WaitForSecondsPauseAware(deathDelay);
        Destroy(gameObject);
    }

    public virtual void OnSelected() { }

    public virtual void OnCommand(CommandType inputCommand, Vector2 target, Entity targetEntity) { }
}
