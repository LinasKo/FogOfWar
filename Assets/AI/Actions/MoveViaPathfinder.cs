using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;

[RAINAction]
public class MoveViapathfinding : RAINAction
{
    public Expression Target = new Expression();
    public Expression Closeness = new Expression();

    private Pathfinding pathfinding;
    private Vector3 pos, navTarget;
    private float closeness;
    private float bodyY;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        pathfinding = ai.Body.GetComponent<Pathfinding>();
        bodyY = ai.Body.transform.position.y;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        pos = ai.Body.transform.position;
        if (Target.IsValid)
        {
            navTarget = Target.Evaluate<GameObject>(ai.DeltaTime, ai.WorkingMemory).transform.position;
        }
        else
        {
            navTarget = ai.WorkingMemory.GetItem<Vector3>("moveTarget");
            navTarget.y = bodyY;
        }

        closeness = Closeness.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);
        if (Vector3.Distance(navTarget, pos) <= closeness)
        {
            return ActionResult.SUCCESS;
        }
        else
        {
            pathfinding.FindPath(pos, navTarget);
            return ActionResult.RUNNING;
        }
    }



    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}