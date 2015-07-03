using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Player { RED, BLUE };

public class GameManager : MonoBehaviour
{
    // Effects camera movement speed.
    public float cameraSpeed = 12.0F;

    // Declare all managers
    private MapManager mapManager;
    private UnitManager unitManager;
    private FogOfWar fog;

    // Effects fog manipulation
    public float fogActionStrength = 1.0f;
    public float fogActionRange = 7.5f;

    // Required for new fog manipulations
    List<Vector3> fogPointList;
    private enum FogControlState { NONE, CLEARING, CREATING };
    private FogControlState fogCtrlState;

    // Fog manipulation restrictions
    private bool canManipulateFog = true;
    private float fogTimeout = 0.5f;

    // Tags of all static objects that need to be rendered according to fog.
    private string[] tagList = new string[] { "Tree", "Building" };

    // TODO find out why it's giving a warning
    private Camera camera;

    private float startTime;

    // GameManager will work for one player
    private Player currentPlayer;

    // Main player stats
    public int playerWood, playerExp, playerHealth;

    // GUI text font variables
    private float _oldWidth;
    private float _oldHeight;
    public float _fontSize;
    public float Ratio = 25;

    // GUI background
    public Texture menuBG1;

    // Use this for initialization
    void Awake()
    {
        // Use the only active camera
        camera = Camera.main;

        // Find the managers
        mapManager = GetComponent<MapManager>();
        unitManager = GetComponent<UnitManager>();
        fog = FogOfWar.FindExisting;

        // Populate the map
        mapManager.MapSetup();

        // Initialize the timer
        startTime = Time.time;

        // Initialize the UnitManager
        unitManager.Initialize();
        InvokeRepeating("SpawnSoldier", 0, 5);
        InvokeRepeating("SpawnGatherer", 0, 10);

        // Initialize input settings
        fogPointList = new List<Vector3>();
        fogCtrlState = FogControlState.NONE;

        // Initialize fog; Remove fog from castle
        List<Vector3> castleList = new List<Vector3>();
        castleList.Add(GameObject.Find("RedCastle").transform.position);
        fog.ManipulateFog(castleList, fogActionStrength, fogActionRange * 2);

        // Initialize resources (NOT NEEDED ATM, USE UNITY INTERFACE)
        // playerWood = 50;
        // playerExp = 0;
        // playerHealth = 100;
    }

    // returns coordinates of current mouse position
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
        // Manage visibility of all tagged static objects.
        foreach (string tag in tagList)
        {
            foreach (GameObject resource in GameObject.FindGameObjectsWithTag(tag))
            {
                if (fog.IsFoggy(resource.transform.position))
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
        Vector3 forwardDirection = Vector3.ProjectOnPlane(camera.transform.up, Vector3.up).normalized * Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime;
        camera.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime, 0F, 0F));
        camera.transform.Translate(forwardDirection, Space.World);

        // Capture the moment when the button was released
        if (Input.GetMouseButtonUp(0))
        {
            switch(fogCtrlState)
            {
                case FogControlState.CLEARING:
                    fog.ManipulateFog(fogPointList, fogActionStrength, fogActionRange);
                    break;
                case FogControlState.CREATING:
                    fog.ManipulateFog(fogPointList, -fogActionStrength, fogActionRange);
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
                if (fog.IsFoggy(mousePosition)) {
                    fogCtrlState = FogControlState.CREATING;
                }
                else
                {
                    fogCtrlState = FogControlState.CLEARING;
                }
            }
        }

        // Manipulate fog based on fogControlState
        if (Input.GetMouseButton(0) && canManipulateFog)
        {
            Vector3 mousePosition = MousePosition();
            if (mousePosition != Vector3.down)
            {
                if ((fogCtrlState == FogControlState.CREATING && fog.IsFoggy(mousePosition) && playerWood >= 25) ||
                    (fogCtrlState == FogControlState.CLEARING && !fog.IsFoggy(mousePosition) && playerWood >= 25))
                {
                    canManipulateFog = false;
                    fogPointList.Add(mousePosition);
                    StartCoroutine(ReallowFogInSeconds(fogTimeout));

                    // Using resources for deletion and creation of fog, plus experience gain
                    playerWood -= 25;
                    playerExp += 10;
                }
            }
        }

        // Script for scaling text (public float _fontSize)
        if (_oldWidth != Screen.width || _oldHeight != Screen.height)
        {
            _oldWidth = Screen.width;
            _oldHeight = Screen.height;
            _fontSize = Mathf.Min(Screen.width, Screen.height) / Ratio;
        }
    }

    IEnumerator ReallowFogInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canManipulateFog = true;
    }

    void SpawnSoldier()
    {
        unitManager.Spawn(UnitType.SOLDIER, Player.RED);
    }

    void SpawnGatherer()
    {
        unitManager.Spawn(UnitType.GATHERER, Player.RED);
    }

    void SpawnEnemySoldier()
    {
        unitManager.Spawn(UnitType.SOLDIER, Player.BLUE);
    }

    void SpawnEnemyGatherer()
    {
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

    public string GetWood()
    {
        return playerWood.ToString();
    }

    public string GetExp()
    {
        return playerExp.ToString();
    }

    public string GetHealth()
    {
        return playerHealth.ToString();
    }

    public void ToMenu()
    {
        Application.LoadLevel("Menu");
    }
}
