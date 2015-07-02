using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowWood : MonoBehaviour
{

    Text wood;

    void Start()
    {
        wood = gameObject.GetComponent<Text>();
        wood.text = GameObject.Find("GameManager").GetComponent<GameManager>().GetWood();
    }

    void Update()
    {
        wood.text = GameObject.Find("GameManager").GetComponent<GameManager>().GetWood();
    }
}