using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using RAIN.Entities;
using RAIN.Entities.Aspects;

[RAINAction]
public class Attack : RAINAction
{
    private int attackDamage;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        attackDamage = ai.WorkingMemory.GetItem<int>("attackDamage");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        GameObject target = ai.WorkingMemory.GetItem<GameObject>("attackableEnemy");
        target.GetComponent<DamageReceiver>().Damage(attackDamage);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}