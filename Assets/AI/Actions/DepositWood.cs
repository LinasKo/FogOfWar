using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;

[RAINAction]
public class DepositWood : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        ai.Body.GetComponent<ResourceCarrier>().UnloadWood();
        ai.WorkingMemory.SetItem<bool>("atCap", ai.Body.GetComponent<ResourceCarrier>().AtCap());
        ai.WorkingMemory.SetItem<bool>("hasWood", ai.Body.GetComponent<ResourceCarrier>().HasWood());
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}