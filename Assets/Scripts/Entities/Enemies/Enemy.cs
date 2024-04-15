using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Entity
{
    protected bool isInCombat = false;

    [SerializeField] private FMODUnity.EventReference fightingSound; 

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Entity" || collision.gameObject.tag == "Player")
        {
            // Enter Combat
            EnterCombat(collision.gameObject.GetComponent<Entity>());
        }
    }

    private void EnterCombat(Entity entity)
    {     
        if (!entity)
            return;

        if (entity.TryGetComponent(out IDamageable damageable))
        {
            isInCombat = true;     

            Vector2 enemyAttackDirection = entity.gameObject.transform.position - transform.position;
            enemyAttackDirection.Normalize();

            entity.TakeDamage(damage);
            AttackAnim(enemyAttackDirection);

            if (entity.canFight)
            {
                Vector2 alliedAttackDirection = transform.position - entity.gameObject.transform.position;
                alliedAttackDirection.Normalize();

                TakeDamage(entity.damage);

                SoundManager.instance.PlaySound(fightingSound);
                entity.AttackAnim(alliedAttackDirection);
            }
        }
    }

    public void Regen()
    {

    }
}
