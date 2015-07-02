using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour
{

    // Added some things just to test rendering (not rendering) in fog of war. These can be deleted if class extends Unit.
    private Renderer rend;
    private FogOfWar2 fog;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        fog = FogOfWar2.FindExisting;
    }

    // Update is called once per frame
    void Update()
    {
        // Lock z axis so it wouldn't roll? right? TODO - find out what I did here.
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        if (fog.IsFoggy(transform.position))
        {
            rend.enabled = false;
        }
        else
        {
            rend.enabled = true;
        }
    }
}
