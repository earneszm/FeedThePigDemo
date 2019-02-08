using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITakeDamage, IEnemy
{
    private int health = 35;

    public int Health { get { return health; } }

    public void TakeDamage(int damage)
    {
        Debug.Log(name + " takes " + damage + " damage");
        health -= damage;

        if (health <= 0)
            Kill();
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
