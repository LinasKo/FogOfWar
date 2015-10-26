using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Player { RED, BLUE };

public class GameManager : MonoBehaviour
{
    // Affects camera movement speed.
    public float cameraSpeed = 12.0F;

    // Define camera zoom speed and zoom limits.
    private float cameraZoomSpeed = 15.0F;
    private float maxZoomOut = 50.0F;
    private float maxZoomIn = 10.0F;

    // Declare all managers
    private MapManager mapManager;
    private UnitManager unitManager;
    private FogOfWar fog_red;
    private FogOfWar fog_blue;

    // Affects fog manipulation
    public float fogActionStrength = 1.0f;
    public float fogActionRange = 7.5f;

    // Required for new fog manipulations
    List<Vector3> fogPointList;
    private enum FogControlState { NONE, CLEARING, CREATING };
    private FogControlState fogCtrlState;
    public LayerMask clickMask;

    // Fog manipulation restrictions
    private bool canManipulateFog = true;
    private float fogTimeout = 0.5f;

    // Tags of all static objects that need to be rendered according to fog.
    private string[] tagList = new string[] { "Tree", "Building" };

    // TODO find out why it's giving a warning
    private Camera mainCamera;

    private float startTime;

    // GameManager will work for one player
    private Player currentPlayer;

    // Main player stats
    public int playerWood_red, playerExp_red, playerHealth_red;
    public int playerWood_blue, playerExp_blue, playerHealth_blue;
    private Vector3 redCastlePos;
    private Vector3 blueCastlePos;

    // GUI background
    public Texture menuBG1;

    // Bot advantage modifiers
    public float botClearRange = 3.3f;
    public float botClearRate = 3;

    // Custom cursor options
    public Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.ForceSoftware;
    private Vector2 hotSpot = Vector2.zero;

    // Use this for initialization
    void Awake()
    {
        // Use the only active camera
        mainCamera = Camera.main;

        // Find the managers
        mapManager = GetComponent<MapManager>();
        unitManager = GetComponent<UnitManager>();
        fog_red  = FogOfWar.FindExisting(Player.RED);
        fog_blue = FogOfWar.FindExisting(Player.BLUE);

        // Populate the map
        mapManager.MapSetup();

        // Find the castles;
        redCastlePos = GameObject.Find("RedCastle").transform.position;
        blueCastlePos = GameObject.Find("BlueCastle").transform.position;

        // Initialize the timer
        startTime = Time.time;
        
        // Initialize the UnitManager
        unitManager.Initialize();

        InvokeRepeating("SpawnSoldierRed", 0, 10);
        InvokeRepeating("SpawnSoldierBlue", 0, 10);
        InvokeRepeating("SpawnGathererRed", 0, 5);
        InvokeRepeating("SpawnGathererBlue", 0, 5);

        // Initialize input settings
        fogCtrlState = FogControlState.NONE;
        fogPointList = new List<Vector3>();

        // Initialize fog; Remove fog from castles
        fogPointList.Add(redCastlePos);
        fog_red.ManipulateFog(fogPointList, fogActionStrength, fogActionRange * 2);
        fogPointList.Clear();

        fogPointList.Add(blueCastlePos);
        fog_blue.ManipulateFog(fogPointList, fogActionStrength, fogActionRange * 2);
        fogPointList.Clear();

        // Initialize some helpful actions for blue player.
        InvokeRepeating("HelpBlue", 0, botClearRate);

        
    }

    // returns coordinates of current mouse position
    Vector3 MousePosition()
    {
        RaycastHit rayHit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayHit, 1000, clickMask))
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
        // Manage visibility (to red player) of all tagged static objects.
        foreach (string tag in tagList)
        {
            foreach (GameObject resource in GameObject.FindGameObjectsWithTag(tag))
            {
                if (fog_red.IsFoggy(resource.transform.position))
                {
                    resource.GetComponent<Renderer>().enabled = false;
                }
                else
                {
                    resource.GetComponent<Renderer>().enabled = true;
                }
            }
        }

        // Move Camera with arrow keys
        Vector3 forwardDirection = Vector3.ProjectOnPlane(mainCamera.transform.up, Vector3.up).normalized * Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime;
        mainCamera.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime, 0F, 0F));
        mainCamera.transform.Translate(forwardDirection, Space.World);


        // Zoom in and out using the mouse scrollwheel
        mainCamera.transform.Translate(Vector3.up * Input.GetAxis("Mouse ScrollWheel") * cameraZoomSpeed * Time.deltaTime, Space.World);
        
        // Zoom in and out using the keypad +/-
        mainCamera.transform.Translate(Vector3.up * Input.GetAxis("Keypad Zoom") * cameraZoomSpeed * Time.deltaTime, Space.World);

        // Do not put camera higher than maxZoomOut or lower than maxZoomIn tresholds.
        if (mainCamera.transform.position.y > maxZoomOut)
        {
            Vector3 currPos = mainCamera.transform.position;
            mainCamera.transform.position = new Vector3(currPos.x, maxZoomOut, currPos.z);
        }

        if (mainCamera.transform.position.y < maxZoomIn)
        {
            Vector3 currPos = mainCamera.transform.position;
            mainCamera.transform.position = new Vector3(currPos.x, maxZoomIn, currPos.z);
        }


        // Manipulate fog for red player.
        // Capture the moment when the button was released
        if (Input.GetMouseButtonUp(0))
        {
            switch(fogCtrlState)
            {
                case FogControlState.CLEARING:
                    fog_red.ManipulateFog(fogPointList, fogActionStrength, fogActionRange);
                    break;
                case FogControlState.CREATING:
                    fog_red.ManipulateFog(fogPointList, -fogActionStrength, fogActionRange);
                    break;
            }
            fogCtrlState = FogControlState.NONE;
        }

        // Capture the moment the button was pressed down
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = MousePosition();
            if (MousePosition() != Vector3.down)
            {
                fogPointList = new List<Vector3>();
                if (fog_red.IsFoggy(mousePosition)) {
                    fogCtrlState = FogControlState.CREATING;
                    Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
                }
                else
                {
                    fogCtrlState = FogControlState.CLEARING;
                    Cursor.SetCursor(null, hotSpot, CursorMode.Auto);
                }
            }
        }

        // Manipulate fog based on fogControlState
        if (Input.GetMouseButton(0) && canManipulateFog)
        {
            Vector3 mousePosition = MousePosition();
            if (mousePosition != Vector3.down)
            {
                if ((fogCtrlState == FogControlState.CREATING && fog_red.IsFoggy(mousePosition) && playerWood_red >= 25) ||
                    (fogCtrlState == FogControlState.CLEARING && !fog_red.IsFoggy(mousePosition) && playerWood_red >= 25))
                {
                    canManipulateFog = false;
                    fogPointList.Add(mousePosition);
                    StartCoroutine(ReallowFogInSeconds(fogTimeout));

                    // Using resources for deletion and creation of fog, plus experience gain
                    playerWood_red -= 25;
                    playerExp_red += 10;
                }
            }
        }
        // Set cursor color
        if (fogCtrlState == FogControlState.NONE)
        {
            if (fog_red.IsFoggy(MousePosition()))
            {
                Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            }
            else
            {
                Cursor.SetCursor(null, hotSpot, CursorMode.Auto);
            }
        }
    }

    IEnumerator ReallowFogInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canManipulateFog = true;
    }

    void SpawnSoldierRed()
    {
        unitManager.Spawn(UnitType.SOLDIER, Player.RED);
    }

    void SpawnSoldierBlue()
    {
        unitManager.Spawn(UnitType.SOLDIER, Player.BLUE);
    }

    void SpawnGathererRed()
    {
        unitManager.Spawn(UnitType.GATHERER, Player.RED);
    }

    void SpawnGathererBlue()
    {
        unitManager.Spawn(UnitType.GATHERER, Player.BLUE);
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

    private void HelpBlue()
    {
        // Clear fog arround Blue's castle every 'botClearRate' seconds, with an increasing range.
        float botHelpFogRange = fogActionRange * 2;
        fog_blue.ManipulateFog(new List<Vector3>() { blueCastlePos }, fogActionStrength, botHelpFogRange);
        botHelpFogRange += botClearRate;
    }

    public string GetWood()
    {
        return playerWood_red.ToString();
    }

    public string GetExp()
    {
        return playerExp_red.ToString();
    }

    public string GetHealth()
    {
        return playerHealth_red.ToString();
    }

    public void ToMenu()
    {
        Application.LoadLevel("Menu");
    }
}
