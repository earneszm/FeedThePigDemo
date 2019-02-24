using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : EffectBase
{
    [SerializeField]
    private float pickupAfterTime;

    private Sprite icon;
    private int goldAmount = 10;

    private void Awake()
    {
        icon = GetComponentInChildren<SpriteRenderer>().sprite;
    }

    private void OnEnable()
    {
        StartCoroutine(LaunchItemAfterTime());
    }

    private IEnumerator LaunchItemAfterTime()
    {
        yield return new WaitForSeconds(pickupAfterTime);

        if (isKillEffect == false)
        {
            Debug.Log("GainGold (LaunchItemAfterTime): " + goldAmount);
            Events.Raise(goldAmount, Camera.main.WorldToScreenPoint(transform.position), icon, GameEventsEnum.EventGoldPickedUp);
        }
        ReturnToPool();
    }

    public override void KillEffect()
    {
        base.KillEffect();
        ReturnToPool();
    }

    public void SetGold(int gold)
    {
        goldAmount = gold;
    }
}
