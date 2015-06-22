using UnityEngine;
using System.Collections;

public class FogManager : MonoBehaviour
{

    public GameObject staticBeacon;
    public GameObject dynamicBeacon;

    public float beaconStrength = 2.0f;
    public float beaconRange = 5.0f;
    
    private FogOfWar fog;

    // Private so that unity would not interfere. Because it sets it to 0 -_-
    private float beaconDelay = 0.33f;

    private GameObject redCastle;
    private GameObject blueCastle;

    private bool canPlaceBeacon;

    // Use this for initialization
    public void Initialize()
    {
        redCastle = GameObject.Find("RedCastle");
        blueCastle = GameObject.Find("BlueCastle");

        fog = FogOfWar.FindExisting;

        canPlaceBeacon = true;


        ClearFogCircle(redCastle.transform.position, 2.0F, true);
        ClearFogCircle(blueCastle.transform.position, 2.0F, true);
    }

    public void ClearFogCircle(Vector3 position, float rangeMultiplier = 1.0F, bool permanent = false)
    {
        if (permanent)
        {
            Beacon beacon = new Beacon(fog, BeaconType.Static, position, beaconStrength, beaconRange * rangeMultiplier);
            fog.AddBeacon(beacon);
        }
        else if (canPlaceBeacon)
        {
            canPlaceBeacon = false;
            Beacon beacon = new Beacon(fog, BeaconType.Dynamic, position, beaconStrength, beaconRange * rangeMultiplier);
            fog.AddBeacon(beacon);
            StartCoroutine(WaitForBeacon(beacon, beaconDelay));
        }
    }

    private IEnumerator WaitForBeacon(Beacon beacon, float time)
    {
        yield return new WaitForSeconds(time);
        canPlaceBeacon = true;
    }

    public bool IsFoggy(Vector3 point)
    {
        Texture2D map = fog.GetViewport().Map;
        Color pixel = map.GetPixelBilinear((point.x + 25.0F) * 0.02F * 1.1F, (point.z + 25.0F) * 0.02F * 1.1F);
        return pixel.a >= 0.5;
    }
}
