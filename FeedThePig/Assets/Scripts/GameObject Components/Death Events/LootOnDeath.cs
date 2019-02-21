using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LootOnDeath : MonoBehaviour, IDeathEvent
{
    [SerializeField]
    private int goldMinAmount;
    [SerializeField]
    private int goldMaxAmount;
    [SerializeField]
    private List<LootItem> possibleLoot;

    public void Raise()
    {
        Events.Raise(Random.Range(goldMinAmount, goldMaxAmount), GameEventsEnum.EventGoldDropped);

        var lootToDrop = GetLootToDrop();

        if (lootToDrop != null)
            Events.Raise(lootToDrop, GameEventsEnum.EventLootDropped);
    }

    private LootItem GetLootToDrop()
    {
        if (possibleLoot == null || possibleLoot.Count == 0)
            return null;

        var totalWeight = possibleLoot.Sum(x => x.GetWeight());

        
        if(Random.Range(1, 100) < totalWeight)
        {
            var selectedLootNum = Random.Range(1, totalWeight);
            var lootChecked = 0f;
            for (int i = 0; i < possibleLoot.Count; i++)
            {
                lootChecked += possibleLoot[i].GetWeight();

                if (lootChecked >= selectedLootNum)
                    return possibleLoot[i];
            }
        }

        return null;
    }


}
