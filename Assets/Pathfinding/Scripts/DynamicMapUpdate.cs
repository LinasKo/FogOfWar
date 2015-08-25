using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicMapUpdate : MonoBehaviour 
{
    public float UpdateTimer = 0.5F;
    private Vector3 lastPosition;
    Bounds lastBounds;

    public Pathfinder pathfinder;
    
    void Start () 
    {
        lastPosition = transform.position;
        lastBounds = GetComponent<Renderer>().bounds;
        UpdateMapOnce();
        StartCoroutine(UpdateMap());
	}	

    IEnumerator UpdateMap()
    {
        if (transform.position != lastPosition)
        {
            Bounds bR = GetComponent<Renderer>().bounds;
            pathfinder.DynamicRaycastUpdate(lastBounds);
            pathfinder.DynamicRaycastUpdate(bR);
            lastPosition = transform.position;
            lastBounds = bR;
        }

        yield return new WaitForSeconds(UpdateTimer);
        StartCoroutine(UpdateMap());
    }

    private void UpdateMapOnce()
    {
        Bounds bR = GetComponent<Renderer>().bounds;
        pathfinder.DynamicRaycastUpdate(lastBounds);
        pathfinder.DynamicRaycastUpdate(bR);
        lastPosition = transform.position;
        lastBounds = bR;
    }

    void OnDestroy()
    {
        UpdateMapOnce();
    }
}
