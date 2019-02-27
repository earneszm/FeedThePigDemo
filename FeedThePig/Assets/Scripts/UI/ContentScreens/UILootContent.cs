using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UILootContent : MonoBehaviour, IIntializeInactive
{
    [SerializeField]
    private Transform uiLootLocation;
    [SerializeField]
    private UILootRow lootRowPrefab;

    //  private List<LootItem> loot;

    public void ForceAwake()
    {

    }

    public void ForceStart()
    {
        Events.Register<LootItem>(GameEventsEnum.EventLootGained, AddLootItemUI);
        Events.Register(GameEventsEnum.EventGameRestart, ClearUI);
    }

    //private void GainLoot(LootItem item)
    //{
    // //   loot.Add(item);
    //    UpdateUI();
    //}

    private void AddLootItemUI(LootItem item)
    {
        var allChildren = uiLootLocation.GetComponentsInChildren<UILootRow>();

        var insertAfter = allChildren.Where(x => x.LootItem.rarity >= item.rarity).LastOrDefault();

        int index = 0;

        if (insertAfter != null)
        {
            index = insertAfter.transform.GetSiblingIndex() + 1;

            if (insertAfter.LootItem.rarity == item.rarity)
            {
                var sameRarityInsertAfter = allChildren.Where(x => x.LootItem.rarity == item.rarity && x.LootItem.Description.CompareTo(item.Description) >= 0).LastOrDefault();

                if (sameRarityInsertAfter != null)
                    index = sameRarityInsertAfter.transform.GetSiblingIndex() + 1;
            }                
        }

        AddItemAtPosition(item, index);
    }

    private void AddItemAtPosition(LootItem item, int index)
    {
        var newItem = Instantiate(lootRowPrefab, uiLootLocation);
        newItem.SetItem(item);

        newItem.transform.SetSiblingIndex(index);
    }

    private void ClearUI()
    {
        var allChildren = uiLootLocation.GetComponentsInChildren<UILootRow>();
        foreach (var item in allChildren)
        {
            Destroy(item.gameObject);
        }
    }

    //private void UpdateUI()
    //{
    //    var allChildren = uiLootLocation.GetComponentsInChildren<UILootRow>();
    //
    //    if (loot.Count == 0)
    //    {
    //        foreach (var item in allChildren)
    //        {
    //            Destroy(item.gameObject);
    //        }
    //    }
    //    else
    //    {
    //        // find any items in memory that are not in the UI and display them
    //        var missingItems = loot.Where(x => !allChildren.Any(c => c.LootItem == x));
    //
    //        foreach (var item in missingItems)
    //        {
    //            var newItem = Instantiate(lootRowPrefab, uiLootLocation);
    //            newItem.SetItem(item);
    //        }
    //    }
    //    
    //}
}
