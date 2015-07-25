using UnityEngine;
using System.Collections;

public class ResourceCarrier : MonoBehaviour
{
    public int woodCap;
    public Player player;

    private int wood = 0;

    private GameManager gameManager;

    public void Start()
    {
        gameManager = (GameManager)(GameObject.Find("GameManager").GetComponent<GameManager>());
    }

    public void TakeWood(int newWood)
    {
        wood += newWood;
    }

    public void UnloadWood(int amount=-1)
    {
        if (amount >= wood || amount == -1)
        {
            if (player == Player.RED)
            {
                gameManager.playerWood_red += wood;
            }
            else
            {
                gameManager.playerWood_blue += wood;
            }
            wood = 0;
        }
        else
        {
            if (player == Player.RED)
            {
                gameManager.playerWood_red += amount;
            }
            else
            {
                gameManager.playerWood_blue += amount;
            }
            wood -= amount;
        }
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