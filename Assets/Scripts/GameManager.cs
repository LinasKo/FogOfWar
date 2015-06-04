using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public MapManager mapManager;
	public FogManager fogManager;

	// Use this for initialization
	void Awake () {
		mapManager = GetComponent<MapManager> ();
		fogManager = GetComponent<FogManager> ();
		InitGame ();
	}

	void InitGame() {
		mapManager.MapSetup();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
