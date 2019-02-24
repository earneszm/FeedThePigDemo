using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDroppable : Droppable
{
    private int goldAmount;

    protected override IEnumerator LaunchItemAfterTime()
    {
        yield return new WaitForSeconds(pickupAfterTime);

        if (isKillEffect == false)
            Events.Raise(goldAmount, (LootItem)null, Camera.main.WorldToScreenPoint(transform.position), sr.sprite, GameEventsEnum.EventLootPickedUp);

        ReturnToPool();
    }

    public void SetGold(int gold)
    {
        goldAmount = gold;
    }
}
