using UnityEngine;
using System.Collections;

public enum UnitType
{
    SOLDIER, GATHERER
};

public abstract class Unit : MonoBehaviour {

    private FogManager fogManager;
    private Renderer rend;

    private int _health, _attackDmg, _attackSpd, _movementSpd;
    private UnitType _type;

    public void Start()
    {
        SetUnit(_health, _attackDmg, _attackSpd, _movementSpd, _type);
        rend = GetComponent<Renderer>();
        fogManager = FindObjectOfType<GameManager>().GetComponent<FogManager>();
    }

    public void Update()
    {
        if (fogManager.IsFoggy(transform.position))
        {
            rend.enabled = false;
        }
        else
        {
            rend.enabled = true;
        }
    }

    public void SetUnit(int health, int attackDmg, int attackSpd, int movementSpd, UnitType type)
    {
        _health = health;
        _attackDmg = attackDmg;
        _attackSpd = attackSpd;
        _movementSpd = movementSpd;
        _type = type;
    }

    public int[] UnitStats()
    {
        int[] list = new int[4];
        list[0] = _health;
        list[1] = _attackDmg;
        list[2] = _attackSpd;
        list[3] = _movementSpd;

        return list;
    }
}
