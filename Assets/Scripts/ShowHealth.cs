using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowHealth : MonoBehaviour
{

    Text health;

    void Start()
    {
        health = gameObject.GetComponent<Text>();
        health.text = GameObject.Find("GameManager").GetComponent<GameManager>().GetHealth();
    }

    void Update()
    {
        health.text = GameObject.Find("GameManager").GetComponent<GameManager>().GetHealth();
    }
}
