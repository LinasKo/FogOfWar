using UnityEngine;
using System.Collections;

public class UnitManager : MonoBehaviour
{

    public float minSpawnDistance;
    public float maxSpawnDistance;

    private Hashtable redUnits;
    private Hashtable blueUnits;

    private bool initialized = false;

    // Use this for initialization
    public void Initialize()
    {
        redUnits = new Hashtable();
        blueUnits = new Hashtable();
        foreach (UnitType type in System.Enum.GetValues(typeof(UnitType)))
        {
            redUnits.Add(type, new ArrayList());
            blueUnits.Add(type, new ArrayList());
        }
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            UpdateUnits(Player.RED);
            UpdateUnits(Player.BLUE);
        }
    }

    // Spawn a unit for a particular player, near a castle
    Unit Spawn(UnitType unit, Player color)
    {
        return null;
        /*
        // Randomly calcualte the spawn distance.
        // TODO: ensure that spawn point is inside the map.
        float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        // Randomly calcualte a spawn angle.
        float spawnAngle = Random.Range(0, Mathf.PI);

        // Get the castle coordinates for a player. TODO: involve more players.
        int castleX = color == Player.RED ? redCastleX : blueCastleX;
        int castleY = color == Player.BLUE ? redCastleY : blueCastleY;

        // Calcualte a point near a castle.
        float spawnX = castleX + spawnDistance * Mathf.Cos(spawnAngle);
        float spawnY = castleY + spawnDistance * Mathf.Sin(spawnAngle);

        // Spawn a unit
        switch (unit)
        {
            case (UnitType.GATHERER):
                return null; // new Gatherer for given player, at (spawnX, spawnY)
                break;
            case (UnitType.SOLDIER):
                return null; // new Solder for given player, at (spawnX, spawnY)
                break;
            default:
                return null;
                break;
        }
        */
    }

    void UpdateUnits(Player playerColor)
    {
        // Update Red units
        if (playerColor.Equals(Player.RED))
        {
            foreach (UnitType type in System.Enum.GetValues(typeof(UnitType)))
            {
                foreach (Unit unit in (ArrayList)redUnits[type])
                {
                    unit.voidUpdateInstance();
                }
            }
        }

        // Update Blue units
        else if (playerColor.Equals(Player.BLUE))
        {
            foreach (UnitType type in System.Enum.GetValues(typeof(UnitType)))
            {
                foreach (Unit unit in (ArrayList)redUnits[type])
                {
                    unit.voidUpdateInstance();
                }
            }
        }
    }
}
