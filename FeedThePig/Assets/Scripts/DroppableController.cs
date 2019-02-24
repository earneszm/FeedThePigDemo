using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableController : MonoBehaviour
{
    [SerializeField]
    private GoldDroppable goldDroppedPrefab;
    [SerializeField]
    private LootDroppable lootDroppedPrefab;

    private void Start()
    {
        Events.Register<int, LootItem, Transform>(GameEventsEnum.EventLootSpawned, CreateDroppableAsLoot);
        Events.Register<int, LootItem, Vector3, Sprite>(GameEventsEnum.EventLootPickedUp, OnLootPickedUp);
    }

    private void CreateDroppableAsLoot(int goldAmount, LootItem lootItem, Transform spawnLocation)
    {
        var droppable = GetPrefab(goldAmount, lootItem, spawnLocation);
        EffectsManager.AddEffect(droppable);
    }

    private void OnLootPickedUp(int amount, LootItem lootItem, Vector3 position, Sprite icon)
    {
        if(lootItem == null)
            Events.Raise<Vector3, Sprite, LaunchTargetLocationEnum, Action>(position, icon, LaunchTargetLocationEnum.Gold, 
                () => { Events.Raise(amount, GameEventsEnum.EventGoldGained); }, GameEventsEnum.EventLaunchItem);
        else
            Events.Raise<Vector3, Sprite, LaunchTargetLocationEnum, Action>(position, icon, LaunchTargetLocationEnum.Default, 
                () => { Events.Raise(lootItem, GameEventsEnum.EventLootGained); }, GameEventsEnum.EventLaunchItem);
    }

    private Droppable GetPrefab(int goldAmount, LootItem lootItem, Transform spawnLocation)
    {
        if (lootItem != null)
        {
            var loot = lootDroppedPrefab.Get<LootDroppable>(spawnLocation.position, Quaternion.identity);
            loot.SetLootItem(lootItem);
            return loot;
        }
        else
        {
            var gold = goldDroppedPrefab.Get<GoldDroppable>(spawnLocation.position, Quaternion.identity);
            gold.SetGold(goldAmount);
            return gold;
        }
    }
}
