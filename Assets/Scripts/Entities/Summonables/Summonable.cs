using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Summonable : Entity
{
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Local Variables
    private Coroutine TESTINGROUTINE = null;

    public override void OnSelected()
    {
        UnitManager.instance.AddToSelected(this);
    }

    /// <summary>
    /// Function that will make the skeleton move towards the target
    /// </summary>
    /// <param name="target"></param>
    protected virtual void MoveTo(Vector2 target)
    {
        if (UnitManager.instance.doDebugLog) Debug.Log("CALLED MOVE TO ON BONEMAN SCRIPT");
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
