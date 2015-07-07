using UnityEngine;
using System.Collections;

public class DamageReceiver : MonoBehaviour
{
    public int health;

    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}

