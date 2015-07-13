using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;

[RAINAction]
public class Loiter : RAINAction
{
    public Expression Allegiance = new Expression();
    public Expression MapHeight = new Expression();
    public Expression MapWidth = new Expression();

    private Player myColor;
    private FogOfWar fog;
    private float mapHeight, mapWidth;

    // This action sets the value "loiterTarget" to the destination if one exists. Returns ActionResult. FAIL otherwise.
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
        
        mapHeight = MapHeight.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);
        mapWidth = MapWidth.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);

        float x, z;
        Vector3 dest = Vector3.zero;
        for (int i = 0; i < 5; i++)
        {
            z = Random.Range(-1f, 1f) * 5 * mapHeight;
            x = Random.Range(-1f, 1f) * 5 * mapWidth;
            dest = new Vector3(x, 0f, z);
            if (!fog.IsFoggy(dest))
            {
                ai.WorkingMemory.SetItem<Vector3>("loiterTarget", dest);
                return ActionResult.SUCCESS;
            }
        }
        return ActionResult.FAILURE;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}