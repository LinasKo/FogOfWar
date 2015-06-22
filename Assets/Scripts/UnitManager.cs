using UnityEngine;
using System.Collections;

public class UnitManager : MonoBehaviour
{

    public string redBase = "RedCastle";
    public string blueBase = "BlueCastle";

    public GameObject RedSoldier;
    public GameObject BlueSoldier;

    public GameObject RedGatherer;
    public GameObject BlueGatherer;

    public float minSpawnDistance;
    public float maxSpawnDistance;

    private GameObject redCastle;
    private GameObject blueCastle;

    private Hashtable redUnits;
    private Hashtable blueUnits;

    private bool initialized = false;

    // Use this for initialization
    public void Initialize()
    {
        redCastle = GameObject.Find(redBase);
        blueCastle = GameObject.Find(blueBase);

        redUnits = new Hashtable();
        blueUnits = new Hashtable();

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
    public GameObject Spawn(UnitType unit, Player color)
    {
        // Randomly calcualte the spawn distance.
        // TODO: ensure that spawn point is inside the map.
        float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        // Randomly calcualte a spawn angle.
        float spawnAngle = Random.Range(0, Mathf.PI);

        // Get the castle coordinates for a player. Also get unit types.
        Vector3 castlePos = new Vector3(0, 0, 0);
        GameObject soldier = null;
        switch (color)
        {
            case Player.RED:
                castlePos = redCastle.transform.position;
                soldier = RedSoldier;
                break;
            case Player.BLUE:
                castlePos = blueCastle.transform.position;
                soldier = BlueSoldier;
                break;
        }

        // Calcualte a point near a castle.
        Vector3 spawn = castlePos + new Vector3(spawnDistance * Mathf.Cos(spawnAngle), 0.5F, spawnDistance * Mathf.Sin(spawnAngle));

        // Spawn a unit


        switch (unit)
        {
            case (UnitType.GATHERER):
                GameObject spawnedGath = Instantiate(soldier, spawn, Quaternion.identity) as GameObject;
                ((ArrayList)redUnits[UnitType.SOLDIER]).Add(spawnedGath);
                return spawnedGath;
            case (UnitType.SOLDIER):
                GameObject spawned = Instantiate(soldier, spawn, Quaternion.identity) as GameObject;
                ((ArrayList)redUnits[UnitType.SOLDIER]).Add(spawned);
                return spawned;
            default:
                return null;
        }

    }

    void UpdateUnits(Player playerColor)
    {

    }
}
