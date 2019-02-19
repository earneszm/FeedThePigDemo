using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour, ITakeDamage, IAttack
{
    [SerializeField]
    private float attackRange = 5;
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float attackSpeed = 2;

    private float lastAttack = 0f;

    private int layerMask;

    private Animal animal;

    public void Initialize(Animal animal)
    {
        this.animal = animal;
    }


    public void TakeDamage(int damage)
    {
        animal.AnimalWeight -= damage;
    }


    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Enemy");
        lastAttack = attackSpeed;

        Events.Register<int>(GameEventsEnum.AnimalSold, OnAnimalSoldChanged);
    }

    // Update is called once per frame
    void Update()
    {        
        var hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), gameObject.transform.right, attackRange, layerMask);

        if (CanMove(hit))
            GameManager.Instance.OnPlayerMovementUpdate(true, speed);
        //  transform.Translate(transform.right * speed * Time.deltaTime);
        else
        {
            GameManager.Instance.OnPlayerMovementUpdate(false, 0);
            TryAttack(hit);
        }
    }

    private void TryAttack(RaycastHit2D hit)
    {
        lastAttack += Time.deltaTime;

        if (lastAttack >= attackSpeed)
        {
            var entity = hit.collider.gameObject.GetComponent<ITakeDamage>();

            if (entity != null)
            {
                lastAttack = 0f;
                Attack(entity);              
            }
        }
    }

    public void Attack(ITakeDamage target)
    {
        target.TakeDamage(animal.Damage);
    }

    // if there is nothing in our range, return true
    // else return false
    private bool CanMove(RaycastHit2D hit)
    {        
        if (hit.collider != null)
            return false;

        return true;
    }

    public void Kill()
    {
        animal.AnimalWeight = 0;
    }

    private void OnAnimalSoldChanged(int weight)
    {
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }
}
