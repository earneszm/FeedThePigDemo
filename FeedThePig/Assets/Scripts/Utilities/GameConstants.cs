using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
    public const float WeightModifier = 1f;
    public const float GoldCostModifier = 0.5f;
    public const float GoldPerWeightPrice = 0.2f;
    public const float GoldRatePerMinute = 5f;
    public const float WeightRatePerMinute = 2f;

    public const float StartingAnimalWeight = 100f;
    public const int StartingAnimalMinDamage = 8;
    public const int StartingAnimalMaxDamage = 11;
    public const int StartingAnimalArmor = 5;
    public const float StartingAnimalSpeed = 10f;
    public const float StartingAnimalCritChance = 20f;
    public const float StartingAnimalCritDamage = 50f;
    public const int StartingGold = 55;

    public const int MaxGoldPerOfflinePeriod = 5000;
    public const float MaxWeightPerOfflinePeriod = 200;
    public const float MaxAnimalWeight = 500f;

    public const float SpawnNewLevelAfterSeconds = 1.5f;

    // chance out of 100 that this rarity would come up (all rarities should sum to 100)
    public static float GetRarityWeight(LootRarityEnum rarity)
    {
        switch (rarity)
        {
            
            case LootRarityEnum.Common:
                return 45f;
            case LootRarityEnum.UnCommon:
                return 35f;
            case LootRarityEnum.Rare:
                return 14f;
            case LootRarityEnum.Epic:
                return 5f;
            case LootRarityEnum.Legendary:
                return 1f;
            default:
            case LootRarityEnum.NotApplicable:
                return 0;
        }
    }
}
