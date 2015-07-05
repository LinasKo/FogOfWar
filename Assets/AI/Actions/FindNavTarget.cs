using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class FindNavTarget : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        Rect bounds = FogOfWar.FindExisting.VisibleRegion;
        Vector3 destination;
        while (true)
        {
            float x = Random.Range(bounds.xMin, bounds.xMax);
            float y = Random.Range(bounds.yMin, bounds.xMax);
            destination = new Vector3(x, 0, y);
            if (!FogOfWar.FindExisting.IsFoggy(destination)) {
                break;
            }
        }
    
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}