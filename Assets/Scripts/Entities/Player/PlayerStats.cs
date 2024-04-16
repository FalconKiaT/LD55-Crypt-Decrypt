using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Entity
{
    [SerializeField] private GameObject deathfade;
    public override void Death()
    {
        base.Death();
        Instantiate(deathfade);
    }
}
