using UnityEngine;
using System.Collections;

public enum Player { RED, BLUE};

public class GameManager : MonoBehaviour {

	public MapManager mapManager;
	public FogManager fogManager;
    public UnitManager unitManager;

    public int redCastleX;
    public int redCastleY;
    public int blueCastleX;
    public int blueCastleY;

    private float startTime;

	// Use this for initialization
	void Awake () {
        mapManager = GetComponent<MapManager> ();
		fogManager = GetComponent<FogManager> ();
        unitManager = GetComponent<UnitManager>();

        // Populate the map
        mapManager.MapSetup(redCastleX, redCastleY, blueCastleX, blueCastleY);

        // Initialize the timer
        startTime = Time.time;

        // Initialize the UnitManager
        unitManager.Initialize(redCastleX, redCastleY, blueCastleX, blueCastleY);

        // TODO: Initialize the fog of war
	}
	
	// Update is called once per frame
	void Update () {
	}
	
}
