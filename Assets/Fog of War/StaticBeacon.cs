using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * This script is associated with StaticBeacon prefabs; you can get a
 * static beacon by instantiating those prefabs, or just associate
 * this script with some other object and you'll get the same effect.
 * Use a static beacon whenever you need to pierce the Fog of War
 * over a region of your map, and you don't expect to move or adjust
 * the beacon often; doing so gives you better performance than just
 * using dynamic beacons all the time.
 */

public class StaticBeacon : MonoBehaviour, IDisposable
{
    /*
     * The Strength of a beacon represents how much it affects the
     * fog of war: positive numbers pierce the fog of war and allow
     * you to see through, while negative numbers draw the fog around
     * the beacon instead and act as shadows.
     */

    [Range(-2.5f, 2.5f)]
    public float Strength = 1f;

    /*
     * The Range of a beacon indicates how far its Strength stretches
     * from its central position.  There's an exponential falloff from
     * the center so the beacon will appear to fade closer to the edges;
     * you can achieve an abrupt cutoff at the edges instead by ramping
     * the Strength field way past the (-2.5 to 2.5) suggested range.
     */

    public float Range = 10f;


    public void OnDestroy() { Update(); }
    public void OnEnable() { Update(); }
    public void OnDisable() { Update(); }

    public void Update()
    {
        // Destroy the Beacon if it has changed or we don't want it any longer
        if (_beacon != null) {
            if (!gameObject.activeInHierarchy ||
                Strength == 0 ||
                Strength != _beacon.Strength ||
                Range != _beacon.Range ||
                transform.position != _beacon.Position) {
                _beacon.Dispose();
                _beacon = null;
            }
        }

        // Create a new Beacon if we still need one
        if (_beacon == null) {
            if (gameObject.activeInHierarchy && Strength != 0) {
                FogOfWar fog = FogOfWar.FindExisting;
                if (fog != null) {
                    _beacon = new Beacon(fog, BeaconType.Static, gameObject.transform.position, Strength, Range);
                }
            }
        }
    }

    public void Dispose()
    {
        if (_beacon != null) {
            _beacon.Dispose();
            _beacon = null;
        }
    }

    protected Beacon _beacon;
}

