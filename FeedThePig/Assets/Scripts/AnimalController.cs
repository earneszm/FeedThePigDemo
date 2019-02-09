using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour, ITakeDamage
{
    [SerializeField]
    private float attackRange = 5;
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float attackSpeed = 2;

    private float lastAttack = 0f;

    private int layerMask;
    

    public void TakeDamage(int damage)
    {
        Debug.Log("Animal is taking damage: " + damage);
    }


    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Enemy");

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
                entity.TakeDamage(10);                
            }
        }
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
        Debug.LogError("Kill not implemented on Animal");
    }

    private void OnAnimalSoldChanged(int weight)
    {
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }
}
