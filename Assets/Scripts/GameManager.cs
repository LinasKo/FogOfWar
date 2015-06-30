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

        // Initialize resources
        playerWood = 100;
        playerExp = 300;
        playerHealth = 100;
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
            if (MousePosition() != Vector3.down)
            {
                if (fogManager.IsFoggy(mousePosition)) {
                    fogCtrlState = FogControlState.CREATING;
                }
                else
                {
                    fogCtrlState = FogControlState.CLEARING;
                }
            }
        }

        // Manipulate fog based on fogControlState
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = MousePosition();
            if (mousePosition != Vector3.down)
            {
                switch (fogCtrlState)
                {
                    case FogControlState.CREATING:
                        if (!fogManager.IsFoggy(mousePosition))
                        {
                            fogManager.CreateFogCircle(mousePosition);
                        }
                        break;
                    case FogControlState.CLEARING:
                        if (!fogManager.IsFoggy(mousePosition))
                        {
                            fogManager.ClearFogCircle(mousePosition);
                        }
                        break;
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

    // BELOW: NOT USED AS IT IS LEGACY CODING STYLE
    // INSTEAD, CREATED A GAMEOBJECT CALLED CANVAS

    // What's displayed when in the gameplay
    /* void OnGUI()
    {
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), menuBG1);

        GUIStyle gs = new GUIStyle();
        gs.fontSize = (int)_fontSize;

        // NEEDS SEPARATE FLOAT VALUES, TO CHANGE
        // THE POSITIONING OF TEXT FOR DIFFERENT
        // SCREEN RESOLUTIONS (where Rect(50f, 20f,..))

        GUI.Label(new Rect(60f, 3f, Screen.width, Screen.height), ("Wood: " + playerWood.ToString()), gs);
        GUI.Label(new Rect(300f, 3f, Screen.width, Screen.height), ("Experience: " + playerExp.ToString()), gs);
        GUI.Label(new Rect(550f, 3f, Screen.width, Screen.height), ("Base health: " + playerHealth.ToString()), gs);
    }
    */

}
