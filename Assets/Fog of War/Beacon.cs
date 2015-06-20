using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * This class allows you to programmatically create Beacons and mess
 * with them.  You can use it from your scripts, or you could just
 * instantiate the StaticBeacon and DynamicBeacon prefabs--either
 * approach works just fine.
 *
 * There are two categories of Beacon objects: static and dynamic.
 * Both have the exact same effect on screen, so technically you can
 * use either one for any purpose.
 *
 * The distinction is because, whenever a beacon is created, destroyed
 * or changes properties, the Fog Of War has to do a lot of thinking
 * in order to advise the shader how to render the scene with the new
 * set of beacons.  The more beacons there are, the more work it has
 * to do--so if you can reduce the number of becaons that have to be
 * thought about at any moment, that's a good thing.
 *
 * If you declare a beacon as type Static, you're saying that you
 * don't expect to have to move it much--like a semi-permanent feature
 * of your world map.  Every time a Static beacon changes we have to
 * think about each and every beacon anywhere, no matter what kind.
 *
 * But if you declare a beacon as type Dynamic, then we don't have
 * to think about all those Static beacons--just the other Dynamic
 * ones.  So create Dynamic beacons just for the objects that are
 * moving a lot, and you won't have to do much computation.
 */

public enum BeaconType
{
    Static,
    Dynamic
};

public class Beacon : IDisposable
{
    public FogOfWar Fog { get; private set; }
    public BeaconType Type { get; private set; }
    public Vector3 Position { get { return _position; } set { _position = value; Fog.UpdateBeacon(this); } }
    public float Strength { get { return _strength; } set { _strength = value; Fog.UpdateBeacon(this); } }
    public float Range { get { return _range; } set { _range = value; Fog.UpdateBeacon(this); } }

    public Beacon (FogOfWar fog, BeaconType type, Vector3 position, float strength, float range)
    {
        Fog = fog;
        Type = type;
        _position = position;
        _strength = strength;
        _range = range;
        Fog.AddBeacon(this);
    }

    public void Dispose()
    {
        Fog.RemoveBeacon(this);
    }

    private float _strength = 1;
    private float _range = 1;
    private Vector3 _position = new Vector3(0,0,0);
}

