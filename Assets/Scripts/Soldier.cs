using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour
{

    // Added some things just to test rendering (not rendering) in fog of war. These can be deleted if class extends Unit.
    private Renderer rend;
    private FogManager fogManager;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        fogManager = FindObjectOfType<GameManager>().GetComponent<FogManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        if (fogManager.IsFoggy(transform.position))
        {
            rend.enabled = false;
        }
        else
        {
            rend.enabled = true;
        }
    }
}
