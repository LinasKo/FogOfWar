using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowExp : MonoBehaviour
{

    Text exp;

    void Start()
    {
        exp = gameObject.GetComponent<Text>();
        exp.text = GameObject.Find("GameManager").GetComponent<GameManager>().GetExp();
    }

    void Update()
    {
        exp.text = GameObject.Find("GameManager").GetComponent<GameManager>().GetExp();
    }
}
