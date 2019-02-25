using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Droppable : EffectBase
{
  //  [SerializeField]
    private float pickupAfterTimeMin = 0.75f;
   // [SerializeField]
    private float pickupAfterTimeMax = 1.5f;
    [SerializeField]
    protected SpriteRenderer sr;

    // protected float pickupAfterTime { get { return Random.Range(pickupAfterTimeMin, pickupAfterTimeMax); } }
    protected float pickupAfterTime = 1f;

    private void Awake()
    {
        pickupAfterTime = Random.Range(pickupAfterTimeMin, pickupAfterTimeMax);
    }

    private void OnEnable()
    {
      //  Events.StartCoroutine(LaunchItemAfterTime());
    }

    public void StartDroppable()
    {
        Events.StartCoroutine(LaunchItemAfterTime());
    }

    protected abstract IEnumerator LaunchItemAfterTime();

    public override void KillEffect()
    {
        base.KillEffect();
        ReturnToPool();
    }    
}   
