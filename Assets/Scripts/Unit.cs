﻿using UnityEngine;
using System.Collections;

public enum UnitType
{
    SOLDIER, GATHERER
};

public abstract class Unit : MonoBehaviour
{
    // Added some things just to test rendering (not rendering) in fog of war. 
    private Renderer rend;
    private FogOfWar fog;
    
    private UnitType _type;

    public void Start()
    {
        rend = GetComponent<Renderer>();
        fog = FogOfWar.FindExisting(Player.RED);
    }

    public void Update()
    {
        if (fog.IsFoggy(transform.position))
        {
            rend.enabled = false;
            foreach (SkinnedMeshRenderer skinRend in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                skinRend.enabled = false;
            }
        }
        else
        {
            rend.enabled = true;
            foreach (SkinnedMeshRenderer skinRend in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                skinRend.enabled = true;
            }
        }
    }

    public void SetUnit(UnitType type)
    {
        _type = type;
    }

    public ArrayList UnitStats()
    {
        ArrayList list = new ArrayList();

        list[0] = _type;

        list.TrimToSize();

        return list;
    }
}
