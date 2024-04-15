using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FKTools;

public class Bomb : Entity
{
    public float fieldofImpact;
    public LayerMask LayersToHit;
    public bool expl = false;
    private bool isExploding = false;
    private bool beingHeld = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public override void FKUpdatePauseAware()
    {
        if(expl && !(isExploding))
        {
            StartCoroutine(FuseRoutinte());
        }
    }

    void Explode()
    {
        isExploding = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, fieldofImpact, LayersToHit);
        
        foreach (Collider2D collider2D in colliders) 
        {
            if (collider2D.tag == "Entity")
            {
                Entity hitEntity = collider2D.GetComponent<Entity>();
                if(hitEntity.TryGetComponent(out IDamageable damageable))
                {
                    hitEntity.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }
    }
    public IEnumerator FuseRoutinte()
    {
        yield return FKRoutines.WaitForSecondsPauseAware(3);
        Explode();
    }

    public override void Death()
    {
        StartCoroutine(FuseRoutinte());
    }
}
