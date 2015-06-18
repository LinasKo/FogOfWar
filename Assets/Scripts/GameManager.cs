using UnityEngine;
using System.Collections;

public enum Player { RED, BLUE };

public class GameManager : MonoBehaviour
{
    private MapManager mapManager;
    private UnitManager unitManager;

    private Camera camera;

    private float startTime;

    // GameManager will work for one player
    private Player currentPlayer;

    // Use this for initialization
    void Start()
    {

        // Use the only active camera
        camera = Camera.main;
        
        mapManager = GetComponent<MapManager>();
        unitManager = GetComponent<UnitManager>();

        // Populate the map
        mapManager.MapSetup();

        // Initialize the timer
        startTime = Time.time;

        // Initialize the UnitManager
        unitManager.Initialize();
        unitManager.Spawn(UnitType.SOLDIER, Player.RED);
        unitManager.Spawn(UnitType.SOLDIER, Player.RED);
        unitManager.Spawn(UnitType.SOLDIER, Player.RED);
        unitManager.Spawn(UnitType.SOLDIER, Player.RED);
        unitManager.Spawn(UnitType.SOLDIER, Player.RED);
        unitManager.Spawn(UnitType.SOLDIER, Player.RED);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
