using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;

[RAINAction]
public class DetectResource : RAINAction
{
    public Expression Range = new Expression();
    public Expression Allegiance = new Expression();

    private Player myColor;
    private FogOfWar fog;
    private Vector3 position;
    private float range;

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
        range = Range.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        position = ai.Kinematic.Position;
        float maxDist = Mathf.Infinity;
        Vector3 closest = Vector3.zero;
        
        foreach (GameObject tree in GameObject.FindGameObjectsWithTag("Tree"))
        {
            float dist = Vector3.Distance(position, tree.transform.position);
            
            if (dist < range && dist < maxDist && !fog.IsFoggy(tree.transform.position))
            {
                maxDist = dist;
                closest = tree.transform.position;
            }
        }
        if (closest != Vector3.zero)
        {
            ai.WorkingMemory.SetItem<bool>("sightedTree", true);
            ai.WorkingMemory.SetItem<Vector3>("moveTarget", closest);
            return ActionResult.SUCCESS;
        }
        return ActionResult.FAILURE;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}