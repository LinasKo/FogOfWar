using UnityEngine;
using System.Collections;

public class PlayerInit : MonoBehaviour {

    public int playerWood, playerExp, playerHealth;

    public GameObject playerCastle;
    public GameObject enemyCastle;
    public Texture frameBox;

    // Use this for initialization
    void Start () {
        playerWood = 100;
        playerExp = 300;
        playerHealth = 100;
    }
	
	// Update is called once per frame
	void Update () {

	}

    void OnGUI () {
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), frameBox);
    }

    void OnMouseOver () {

    }
}
