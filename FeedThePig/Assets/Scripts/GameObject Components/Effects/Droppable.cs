using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Droppable : EffectBase
{
    [SerializeField]
    protected float pickupAfterTime = 1f;    
    [SerializeField]
    protected SpriteRenderer sr;
    
    private void OnEnable()
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
