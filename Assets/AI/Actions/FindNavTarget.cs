using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;

[RAINAction]
public class FindNavTarget : RAINAction
{
    public Expression Allegiance = new Expression();
    private Player myColor;
    private FogOfWar fog;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        if (Allegiance.Evaluate<string>(ai.DeltaTime, ai.WorkingMemory) == "Red")
        {
            myColor = Player.RED;
        }
        else
        {
            myColor = Player.BLUE;
        }
        fog = FogOfWar.FindExisting(myColor);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        Rect bounds = fog.VisibleRegion;
        Vector3 destination;
        while (true)
        {
            float x = Random.Range(bounds.xMin, bounds.xMax);
            float y = Random.Range(bounds.yMin, bounds.xMax);
            destination = new Vector3(x, 0, y);
            if (!fog.IsFoggy(destination)) {
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