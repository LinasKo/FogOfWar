using UnityEngine;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class Die : RAINAction
{
    public override void Start(AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
        MonoBehaviour.Destroy(ai.Body);

        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}