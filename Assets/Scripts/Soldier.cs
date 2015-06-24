using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour {

    // Added some things just to test rendering (not rendering) in fog of war. These can be deleted if class extends Unit.
    private Renderer rend;
    private FogManager fogManager;
    bool startUpdating = false;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        fogManager = FindObjectOfType<GameManager>().GetComponent<FogManager>();
        StartCoroutine(AllowRender());
    }

    // Waits in background for 1 second and the allow to hide units in fog.
    // This is because Viewport takes time to be created, for some reason. TODO: fix dis.
    private IEnumerator AllowRender()
    {
        yield return new WaitForSeconds(0.1f);
        startUpdating = true;
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);

        if (startUpdating)
        {
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
}
