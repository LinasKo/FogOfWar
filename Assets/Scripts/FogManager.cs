using UnityEngine;
using System.Collections;

public class FogManager : MonoBehaviour {

    public GameObject staticBeacon;
    public GameObject dynamicBeacon;

    public FogOfWar fog;

    public float beaconStrength = 2.0f;
    public float beaconRange = 5.0f;

    private GameObject redCastle;
    private GameObject blueCastle;

    // Use this for initialization
    public void Initialize () {

        redCastle = GameObject.Find("RedCastle");
        blueCastle = GameObject.Find("BlueCastle");

        ClearFogCircle(redCastle.transform.position, 2.0F);
        ClearFogCircle(blueCastle.transform.position, 2.0F);
    }

    public void ClearFogCircle(Vector3 position, float rangeMultiplier = 1.0F)
    {
        Beacon beacon = new Beacon(fog, BeaconType.Static, position, beaconStrength, beaconRange * rangeMultiplier);
        fog.AddBeacon(beacon);
    }

    public bool IsFoggy(Vector3 point)
    {
        Texture2D map = fog.GetViewport().Map;
        Color pixel = map.GetPixelBilinear((point.x + 25.0F) * 0.02F* 1.1F, (point.z + 25.0F) * 0.02F * 1.1F);
        return pixel.a >= 0.5;
    }
}
