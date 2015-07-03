using UnityEngine;
using System.Collections;

public class Gatherer : Unit
{
    public int gatHealth, gatSpeed;
    private Unit gatherer;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        gatherer = gameObject.GetComponent<Unit>();
        gatherer.SetUnit(gatHealth, 0, 0, gatSpeed, UnitType.GATHERER);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
