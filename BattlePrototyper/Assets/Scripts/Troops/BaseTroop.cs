using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTroop : MonoBehaviour
{
    public string troopName;

    public float health;

    public float attack;

    public float range;

    public void AttackTarget(BaseTroop troop)
    {
        troop.health -= attack;
    }
}
