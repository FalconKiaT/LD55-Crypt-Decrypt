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
    [SerializeField] public ParticleSystem testParticleSystem;
    public SpriteRenderer rend;

    private Coroutine ExplosionRoutine = null;

    private BobbingObject bobbingObject;

    private void Start()
    {
        bobbingObject = GetComponentInChildren<BobbingObject>();
        rb = GetComponent<Rigidbody2D>();
        
        //var em = testParticleSystem.emission;
        //em.enabled = true;
        //testParticleSystem.Play();
        //Debug.Log("Played)");
    }

    // Update is called once per frame
    public override void FKUpdatePauseAware()
    {
        if (ExplosionRoutine != null) return;
        if(expl && !(isExploding))
        {
            ExplosionRoutine = StartCoroutine(FuseRoutinte());

        }
    }

    public void HasBeenGrabbed()
    {
        bobbingObject.canBob = false;
    }

    void Explode()
    {
        isExploding = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, fieldofImpact, LayersToHit);
        rend.enabled = !rend.enabled;
        testParticleSystem.Play();
        foreach (Collider2D collider2D in colliders) 
        {
            if (collider2D.tag == "Entity")
            {
                Entity hitEntity = collider2D.GetComponent<Entity>();
                if(hitEntity.TryGetComponent(out IDamageable damageable))
                {
                    
                    
                    hitEntity.TakeDamage(damage);
                    
                }
            }
        }
        
    }
    public IEnumerator FuseRoutinte()
    {
        yield return FKRoutines.WaitForSecondsPauseAware(3);
        //Debug.Log("particle");
        
        Explode();
        yield return FKRoutines.WaitForSecondsPauseAware(2);
        Destroy(gameObject);
    }

    public override void Death()
    {
        StartCoroutine(FuseRoutinte());
    }
}
