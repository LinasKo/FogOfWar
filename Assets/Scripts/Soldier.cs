using UnityEngine;
using System.Collections;

public class Soldier : Unit
{
    public int soldHealth, soldAtt, soldAttSpd, soldSpeed;
    private Unit soldier;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        soldier = gameObject.GetComponent<Unit>();
        soldier.SetUnit(soldHealth, soldAtt, soldAttSpd, soldSpeed, UnitType.SOLDIER);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
