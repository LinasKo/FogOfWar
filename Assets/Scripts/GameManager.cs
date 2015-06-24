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
    void Awake()
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
        Vector3 forwardDirection = Vector3.ProjectOnPlane(camera.transform.up, Vector3.up).normalized * Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime;
        camera.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime, 0F, 0F));
        camera.transform.Translate(forwardDirection, Space.World);

        //   Manipulate fog of war

        // Capture the moment when the button was released
        if (Input.GetMouseButtonUp(0))
        {
            fogCtrlState = FogControlState.NONE;
        }

        // Capture the moment the button was pressed down
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = MousePosition();
            // TODO if foggy:
            // TODO fogCtrlState = FogControlState.CREATING;
            Debug.Log("" + mousePosition + ", " + fogManager.IsFoggy(mousePosition) + ", " + (mousePosition.x + 25.0F) * 0.02F + ", " + (mousePosition.z + 25.0F) * 0.02F +
                "\n" + FogOfWar.FindExisting.GetViewport().Map.GetPixelBilinear((mousePosition.x + 25.0F) * 0.02F, (mousePosition.z + 25.0F) * 0.02F));
            if (MousePosition() != Vector3.down && !fogManager.IsFoggy(mousePosition))
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
                    if (mousePosition != Vector3.down && !fogManager.IsFoggy(mousePosition))
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

    public ArrayList ObjectsInCircle(Vector3 coordinates, float radius, string tag = null)
    {
        coordinates.y = 0;
        Collider[] colliders = Physics.OverlapSphere(coordinates, radius);
        ArrayList objects = new ArrayList();
        foreach (Collider col in colliders)
        {
            GameObject gameObject = col.gameObject;
            if (tag == null || gameObject.tag == tag)
            {
                objects.Add(gameObject);
            }
        }
        return objects;
    }
}
