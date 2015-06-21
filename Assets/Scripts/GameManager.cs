using UnityEngine;
using System.Collections;

public enum Player { RED, BLUE };

public class GameManager : MonoBehaviour
{
    public float cameraSpeed = 12.0F;

    private MapManager mapManager;
    private UnitManager unitManager;
    private FogManager fogManager;

    private Camera camera;

    private float startTime;

    // GameManager will work for one player
    private Player currentPlayer;

    private enum FogControlState { NONE, CLEARING, CREATING };
    private FogControlState fogCtrlState;

    // Use this for initialization
    void Start()
    {

        // Use the only active camera
        camera = Camera.main;

        mapManager = GetComponent<MapManager>();
        unitManager = GetComponent<UnitManager>();
        fogManager = GetComponent<FogManager>();

        // Populate the map
        mapManager.MapSetup();

        // Initialize the timer
        startTime = Time.time;

        // Initialize the UnitManager
        unitManager.Initialize();
        InvokeRepeating("SpawnSoldiers", 0, 5);

        // Initialize input settings
        fogCtrlState = FogControlState.NONE;

        // Initialize fog
        fogManager.Initialize();
    }

    Vector3 MousePosition()
    {
        RaycastHit rayHit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayHit, 1000))
        {
            return rayHit.point;
        }
        else
        {
            return Vector3.down;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move Camera with arrow keys
        camera.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime, Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime, 0F));

        //   Manipulate fog of war

        // Capture the moment when the button was released
        if (Input.GetMouseButtonUp(0))
        {
            fogCtrlState = FogControlState.NONE;
        }

        // Capture the moment the button was pressed down
        if (Input.GetMouseButtonDown(0))
        {
            // TODO if foggy:
            // TODO fogCtrlState = FogControlState.CREATING;
            // TODO if not foggy
            if (MousePosition() != Vector3.down)
            {
                fogCtrlState = FogControlState.CLEARING;
            }
        }

        // Manipulate fog based on fogCtrlState
        if (Input.GetMouseButton(0))
        {
            switch (fogCtrlState)
            {
                // TODO take player into account
                case FogControlState.CREATING:
                    // TODO
                    break;
                case FogControlState.CLEARING:
                    Vector3 mousePosition = MousePosition();
                    if (MousePosition() != Vector3.down)
                    {
                        fogManager.ClearFogCircle(mousePosition);
                    }
                    break;
            }
        }

    }

    void SpawnSoldiers()
    {
        unitManager.Spawn(UnitType.SOLDIER, Player.RED);
        unitManager.Spawn(UnitType.SOLDIER, Player.BLUE);
    }
}
