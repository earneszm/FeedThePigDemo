using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot", menuName = "Scriptables/Loot")]
public class LootItem : ScriptableObject
{
    public string Description;
    public Sprite Icon;    
    public float DropWeight = 50f;
    public LaunchTargetLocationEnum onPickupGoTo;
    public LootRarityEnum rarity;

    [Header("Modifiers")]
    public int Damage = 0;
    public float CritChance = 0f;
    public float CritDamage = 0f;
    public int Armor = 0;
    public float Speed = 0f;

    public float GetWeight()
    {
        return GameConstants.GetRarityWeight(rarity) + DropWeight;
    }

    public string GetStatsDescription()
    {
        var returnString = string.Format("{0}{1}{2}{3}{4}",
            Damage > 0 ? ("Damage +" + Damage + ", ") : "",
            CritChance > 0 ? ("CritChance +" + CritChance + ", ") : "",
            CritDamage > 0 ? ("CritDamage +" + CritDamage + ", ") : "",
            Armor > 0 ? ("Armor +" + Armor + ", ") : "",
            Speed > 0 ? ("Speed +" + Speed + ", ") : "");

        return returnString.Substring(0, returnString.Length - 2);

    }
}
