using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public MeshRenderer redFogRenderer;
	public MeshRenderer blueFogRenderer;

	// Use this for initialization
	void Start () {
		redFogRenderer.enabled = false;
		blueFogRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
