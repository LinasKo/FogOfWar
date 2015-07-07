using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;

[RAINAction]
public class DetectEnemy : RAINAction
{
    public Expression Range = new Expression();
    public Expression TargetAllegiance = new Expression();

    private UnitManager unitManager;
    private FogOfWar fog;
    private Vector3 position;
    private float range;
    private Player targetColor;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        unitManager = (UnitManager)(GameObject.Find("GameManager").GetComponent<UnitManager>());
        fog = FogOfWar.FindExisting;
        range = Range.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);
        targetColor = TargetAllegiance.Evaluate<string>(ai.DeltaTime, ai.WorkingMemory) == "Red" ? Player.RED : Player.BLUE;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        position = ai.Kinematic.Position;
        float maxDist = Mathf.Infinity;
        Vector3 closest = Vector3.zero;
        
        foreach (GameObject unit in unitManager.GetUnitList(targetColor))
        {
            if (unit == null)
            {
                continue;
            }
            float dist = Vector3.Distance(position, unit.transform.position);
            
            if (dist < range && dist < maxDist && !fog.IsFoggy(unit.transform.position))
            {
                maxDist = dist;
                closest = unit.transform.position;
            }
        }
        if (closest != Vector3.zero)
        {
            ai.WorkingMemory.SetItem<Vector3>("sightedEnemy", closest);
            return ActionResult.SUCCESS;
        }
        return ActionResult.FAILURE;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}