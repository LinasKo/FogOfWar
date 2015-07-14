﻿using UnityEngine;
using System.Collections.Generic;

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

    private Dictionary<Player, List<GameObject>> unitList;

    private bool initialized = false;

    // Use this for initialization
    public void Initialize()
    {
        redCastle = GameObject.Find(redBase);
        blueCastle = GameObject.Find(blueBase);

        unitList = new Dictionary<Player, List<GameObject>>();

        foreach (Player color in System.Enum.GetValues(typeof(Player)))
        {
            unitList.Add(color, new List<GameObject>());
        }
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {

        // For debugging - destroy castle when health reaches 0
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<GameManager>().playerHealth_red -= 25;
        }
        if (GetComponent<GameManager>().playerHealth_red <= 0)
        {
            Destroy(redCastle);
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

        GameObject soldier = RedSoldier;
        GameObject gatherer = RedGatherer;

        switch (color)
        {
            case Player.RED:
                if (redCastle == null)
                {
                    return null;
                }
                castlePos = redCastle.transform.position;
                soldier = RedSoldier;
                gatherer = RedGatherer;
                break;
            case Player.BLUE:
                if (blueCastle == null)
                {
                    return null;
                }
                castlePos = blueCastle.transform.position;
                soldier = BlueSoldier;
                gatherer = BlueGatherer;
                break;
        }

        // Calculate a point near a castle.
        Vector3 spawn = castlePos + new Vector3(spawnDistance * Mathf.Cos(spawnAngle), 0, spawnDistance * Mathf.Sin(spawnAngle));

        // Spawn a unit
        GameObject spawned;
        switch (unit)
        {
            case (UnitType.GATHERER):
                spawned = Instantiate(gatherer, spawn, Quaternion.identity) as GameObject;
                unitList[color].Add(spawned);
                return spawned;
            case (UnitType.SOLDIER):
                spawned = Instantiate(soldier, spawn, Quaternion.identity) as GameObject;
                unitList[color].Add(spawned);
                return spawned;
            default:
                return null;
        }

    }

    public List<GameObject> GetUnitList(Player color)
    {
        return unitList[color];
    }
}
