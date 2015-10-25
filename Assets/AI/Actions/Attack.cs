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
        Vector3 rotationVector = target.transform.position - ai.Body.transform.position;
        if (rotationVector != Vector3.zero)
        {
            ai.Body.transform.rotation = Quaternion.LookRotation(rotationVector);
        }

        // Deal damage to enemy
        target.GetComponent<DamageReceiver>().Damage(attackDamage, ai.Body.transform.position);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}