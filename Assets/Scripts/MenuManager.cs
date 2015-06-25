using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MovieTexture movie = (MovieTexture)GetComponent<MeshRenderer>().material.mainTexture;
        movie.loop = true;
        movie.Play();
    }

	// Update is called once per frame
	void Update () {

    }
}
