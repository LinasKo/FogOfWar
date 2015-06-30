using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public Texture trololo;
    private bool displayTrololo = false;

	// Use this for initialization
	void Start()
    {
        MovieTexture movie = (MovieTexture)GetComponent<MeshRenderer>().material.mainTexture;
        movie.loop = true;
        movie.Play();
    }

	// Update is called once per frame
	void Update()
    {

    }

    public void Trollface()
    {
        displayTrololo = true;
    }

    public void DisableTroll()
    {
        displayTrololo = false;
    }

    public void OnGUI()
    {
        if (displayTrololo == true)
        {
            GUI.DrawTexture(new Rect(100f, 50f, 400, 325), trololo);
            OnMouseDown();
        }
    }

    void OnMouseDown()
    {
        if (displayTrololo == false)
        {
            GUIElement.Destroy(trololo);
        }
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
