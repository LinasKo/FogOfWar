using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {

    private MovieTexture movie;

    void Awake()
    {
        movie = (MovieTexture)GetComponent<MeshRenderer>().material.mainTexture;
    }

	void Start()
    {
        movie.loop = true;
        movie.Play();
    }

    public void LaunchGame()
    {
        Application.LoadLevel("Fog");
    }

    public void ToMenu()
    {
        Application.LoadLevel("Menu");
    }

    public void ToOptions()
    {
        Application.LoadLevel("MenuOptions");
    }
}
