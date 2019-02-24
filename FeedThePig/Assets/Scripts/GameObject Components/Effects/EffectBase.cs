using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectBase : PooledMonoBehaviour
{
    protected bool isKillEffect;

    public virtual void KillEffect()
    {
        isKillEffect = true;
    }

    protected void OnEffectActive()
    {
        isKillEffect = false;
    }

    protected override void ReturnToPool(float delay = 0)
    {
        EffectsManager.RemoveEffect(this);
        base.ReturnToPool(delay);
    }

    private void OnEnable()
    {
        OnEffectActive();
    }
}
