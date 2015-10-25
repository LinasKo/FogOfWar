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
public class Cut : RAINAction
{
    private int attackDamage;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        attackDamage = ai.WorkingMemory.GetItem<int>("attackDamage");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        GameObject target = ai.WorkingMemory.GetItem<GameObject>("cuttableTree");
        target.GetComponent<DamageReceiver>().Damage(attackDamage, ai.Body.transform.position);
        ai.Body.GetComponent<ResourceCarrier>().TakeWood(attackDamage);
        ai.WorkingMemory.SetItem<bool>("atCap", ai.Body.GetComponent<ResourceCarrier>().AtCap());
        ai.WorkingMemory.SetItem<bool>("hasWood", ai.Body.GetComponent<ResourceCarrier>().HasWood());
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}