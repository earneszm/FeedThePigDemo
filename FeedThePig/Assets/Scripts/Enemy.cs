using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, ITakeDamage, IEnemy, IAttack
{
    [SerializeField]
    private int health = 35;
    [SerializeField]
    private int maxHealth = 35;    
    [SerializeField]
    private float attackSpeed = 3f;
    [SerializeField]
    private int minAttackDamage = 1;
    [SerializeField]
    private int maxAttackDamage = 5;
    [SerializeField]
    private Image healthImage;

    public int Health { get { return health; } }
    public int MaxHealth { get { return maxHealth; } }

    private float distanceThreshold = 1f;
    private float speed = 2f;

    private float lastAttack = 0f;

    private bool isDoneMoving;

    private ITakeDamage animalTarget;

    public void Initialize(ITakeDamage target)
    {
        animalTarget = target;
    }

    public void Update()
    {
        if (isDoneMoving == false)
            return;

        TryAttack();
    }

    private void TryAttack()
    {
        lastAttack += Time.deltaTime;

        if (lastAttack >= attackSpeed)
        {
            lastAttack = 0f;
            Attack(animalTarget);
        }
    }

    public void TakeDamage(int damage)
    {
       // Debug.Log(name + " takes " + damage + " damage");
        health -= damage;

        UpdateHealthBar();

        if (health <= 0)
            Kill();
    }

    public void Kill()
    {
        var deathEvents = GetComponents<IDeathEvent>();

        foreach (var deathEvent in deathEvents)
        {
            deathEvent.Raise();
        }

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
        {
            isDoneMoving = true;
         //   Debug.Log("Enemy is done moving");
        }
    }

    public void Attack(ITakeDamage target)
    {
       // Debug.Log("Enemy is attacking for damage: " + attackDamage);
        target.TakeDamage(Random.Range(minAttackDamage, maxAttackDamage));
    }

    private void UpdateHealthBar()
    {
        healthImage.fillAmount = (float)health / (float)maxHealth;
    }
}
