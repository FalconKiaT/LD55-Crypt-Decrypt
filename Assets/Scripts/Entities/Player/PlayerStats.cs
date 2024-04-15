using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Entity
{
    public override void Death()
    {
        // TODO: Death Stuff
        Destroy(gameObject);
    }
}
