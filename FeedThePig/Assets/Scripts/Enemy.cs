using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITakeDamage, IEnemy
{
    private int health = 35;

    public int Health { get { return health; } }

    private float distanceThreshold = 1f;
    private float speed = 2f;

    private bool isDoneMoving;

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

    public void TryMove(Transform target)
    {
        if (isDoneMoving)
            return;

        if (Vector3.Distance(transform.position, target.position) > distanceThreshold)
        {
            // move
            transform.Translate(-transform.right * speed * Time.deltaTime);
        }
        else
            isDoneMoving = true;
    }
}
