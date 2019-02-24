using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDroppable : Droppable
{
    private LootItem lootItem;

    protected override IEnumerator LaunchItemAfterTime()
    {
        yield return new WaitForSeconds(pickupAfterTime);

        if (isKillEffect == false)
            Events.Raise(0, lootItem, Camera.main.WorldToScreenPoint(transform.position), sr.sprite, GameEventsEnum.EventLootPickedUp);

        ReturnToPool();
    }

    public void SetLootItem(LootItem item)
    {
        lootItem = item;

        if(lootItem.Icon != null)
            sr.sprite = lootItem.Icon;
    }
}
