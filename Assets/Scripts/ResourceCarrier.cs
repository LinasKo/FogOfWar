using UnityEngine;
using System.Collections;

public class ResourceCarrier : MonoBehaviour
{
    public int woodCap;
    private int wood = 0;
    private bool hasWood = false;

    private GameManager gameManager;

    public void Start()
    {
        gameManager = (GameManager)(GameObject.Find("GameManager").GetComponent<GameManager>());
    }

    public void TakeWood(int newWood)
    {
        wood += newWood;
    }

    public void UnloadWood(int amount=0)
    {
        amount = amount > wood ? wood : amount;
        amount = amount == 0 ? wood : amount;

        wood -= amount;
        gameManager.playerWood += amount;
    }

    public bool AtCap()
    {
        return wood >= woodCap;
    }

    public bool HasWood()
    {
        return wood > 0;
    }


}

