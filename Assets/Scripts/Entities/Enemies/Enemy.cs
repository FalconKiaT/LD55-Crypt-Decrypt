using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private bool isInCombat = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Entity")
        {
            // Enter Combat
            EnterCombat(collision.gameObject.GetComponent<Entity>());
        }
    }

    private void EnterCombat(Entity entity)
    {
        isInCombat = true;

        if (!entity)
            return;

        if (entity.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(health);
            TakeDamage(entity.damage);
        }
    }

    public void Regen()
    {

    }
}
