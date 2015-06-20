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

        CreateFogCircle(redCastle.transform.position, 2.0F);
        CreateFogCircle(blueCastle.transform.position, 2.0F);

        Debug.Log("sefsef");
    }

    public void CreateFogCircle(Vector3 position, float rangeMultiplier = 1.0F)
    {
        Beacon beacon = new Beacon(fog, BeaconType.Static, position, beaconStrength, beaconRange * rangeMultiplier);
        fog.AddBeacon(beacon);
    }
}
