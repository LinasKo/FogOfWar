using UnityEngine;
using System.Collections;

public enum UnitType
{
    SOLDIER, GATHERER
};

public abstract class Unit : MonoBehaviour
{
    // private FogManager fogManager;

    // Added some things just to test rendering (not rendering) in fog of war. 
    private Renderer rend;
    private FogOfWar fog;

    private int _health, _attackDmg, _attackSpd, _movementSpd;
    private UnitType _type;

    public void Start()
    {
        rend = GetComponent<Renderer>();
        fog = FogOfWar.FindExisting;
    }

    public void Update()
    {
        // Lock z axis so it wouldn't roll? right? TODO - find out what I did here.
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        if (fog.IsFoggy(transform.position))
        {
            rend.enabled = false;
        }
        else
        {
            rend.enabled = true;
        }
    }

    public void SetUnit(int health, int attackDmg, int attackSpd, int movementSpd, UnitType type)
    {
        _health = health;
        _attackDmg = attackDmg;
        _attackSpd = attackSpd;
        _movementSpd = movementSpd;
        _type = type;
    }

    public ArrayList UnitStats()
    {
        ArrayList list = new ArrayList();

        list[0] = _health;
        list[1] = _attackDmg;
        list[2] = _attackSpd;
        list[3] = _movementSpd;
        list[4] = _type;

        list.TrimToSize();

        return list;
    }
}
