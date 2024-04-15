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
    SmallEnemy
}

public class Entity : FKMonoBehaviour
{
    public EntityTypes entityType;
}
