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
        // Find enemy
        GameObject target = ai.WorkingMemory.GetItem<GameObject>("attackableEnemy");

        // Turn to face enemy
        ai.Body.transform.rotation = Quaternion.LookRotation(target.transform.position - ai.Body.transform.position);

        // Deal damage to enemy
        target.GetComponent<DamageReceiver>().Damage(attackDamage);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}