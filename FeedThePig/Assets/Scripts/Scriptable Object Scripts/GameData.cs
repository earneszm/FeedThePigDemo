using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

[CreateAssetMenu(fileName = "gameData", menuName = "Scriptables/gameData")]
public class GameData : ScriptableObject, IGoldRate
{
    #region Serialized Data
    [SerializeField] private bool isExistingUser;
    [SerializeField] private int animalsSold;
    [SerializeField] private int totalFoodBought;
    [SerializeField] private float totalWeightAcquired;
    [SerializeField] private long lastLoginTime;
    [SerializeField] private long lastAppClosedTime;

    [SerializeField] private int gold = GameConstants.StartingGold;
    [SerializeField] private float goldCostModifier = GameConstants.GoldCostModifier;
    [SerializeField] private float weightModifier = GameConstants.WeightModifier;
    [SerializeField] private float goldRatePerMinute = GameConstants.GoldRatePerMinute;
    [SerializeField] private float weightRatePerMinute = GameConstants.WeightRatePerMinute;
    [SerializeField] private int maxGoldPerOfflinePeriod = GameConstants.MaxGoldPerOfflinePeriod;
    [SerializeField] private float maxWeightPerOfflinePeriod = GameConstants.MaxWeightPerOfflinePeriod;


    public float goldPerWeightPrice = GameConstants.GoldPerWeightPrice;
    public int CurrentLevel;   
    public Animal Animal;

    public float PlayerDistance;
    public bool IsPlayerMoving;

    public List<int> PurchasedUpgradeIDs = new List<int>();    
    public List<Enemy> CurrentEnemies = new List<Enemy>();
    public List<LootItem> Loot = new List<LootItem>();
    #endregion

    #region Properties

    public bool IsExistingUser             { get { return isExistingUser;                  } private set { isExistingUser = value; } }
    public int MaxGoldPerOfflinePeriod     { get { return maxGoldPerOfflinePeriod;         } }
    public float MaxWeightPerOfflinePeriod { get { return maxWeightPerOfflinePeriod;       } }
    public DateTime LastLoginTime          { get { return new DateTime(lastLoginTime);     } private set { lastLoginTime = value.Ticks; } }
    public DateTime LastAppClosedTime      { get { return new DateTime(lastAppClosedTime); } private set { lastAppClosedTime = value.Ticks; } }

    // Properties with Events
    public int   Gold                  { get { return gold;                } set { gold = value;                Events.OnChange(gold,                GameEventsEnum.Gold);                } }        
    public int   AnimalsSold           { get { return animalsSold;         } set { animalsSold = value;         Events.OnChange(animalsSold,         GameEventsEnum.AnimalSold);          } }    
    public int   TotalFoodBought       { get { return totalFoodBought;     } set { totalFoodBought = value;     Events.OnChange(totalFoodBought,     GameEventsEnum.TotalFoodBought);     } }
    public float TotalWeightAcquired   { get { return totalWeightAcquired; } set { totalWeightAcquired = value; Events.OnChange(totalWeightAcquired, GameEventsEnum.TotalWeightAcquired); } }
    public float GoldCostModifier      { get { return goldCostModifier;    } set { goldCostModifier = value;    Events.OnChange(goldCostModifier,    GameEventsEnum.GoldCostModifier);    } }
    public float WeightModifier        { get { return weightModifier;      } set { weightModifier = value;      Events.OnChange(weightModifier,      GameEventsEnum.WeightModifier);      } }
    public float GoldRatePerMinute     { get { return goldRatePerMinute;   } set { goldRatePerMinute = value;   Events.OnChange(goldRatePerMinute,   GameEventsEnum.GoldProduction);      } }
    public float WeightRatePerMinute   { get { return weightRatePerMinute; } set { weightRatePerMinute = value; Events.OnChange(weightRatePerMinute, GameEventsEnum.WeightProduction);    } }

    #endregion

    #region Helper Methods

    public void UpdateAnimal(Animal animal)
    {
        Animal = animal;
    }

    public void OnApplicationOpen()
    {
        LastLoginTime = DateTime.Now;
        ForceDataBind();
    }

    public void OnApplicationQuit()
    {
        CurrentEnemies.Clear();
        IsExistingUser = true;
        LastAppClosedTime = DateTime.Now;
    }

    public void ForceDataBind()
    {        
        Animal.ForceDataBind();

        Gold += 0;
        GoldCostModifier += 0;
        WeightModifier += 0;
        GoldRatePerMinute += 0;
        AnimalsSold += 0;
        TotalFoodBought += 0;
        TotalWeightAcquired += 0;

        foreach (var upgradeID in PurchasedUpgradeIDs)
        {
            Events.OnChange(upgradeID, GameEventsEnum.Upgrade);
        }
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }

    public void AddUpgrade(UpgradeShopItem upgrade)
    {        
        PurchasedUpgradeIDs.Add(upgrade.GetInstanceID());

        Events.OnChange(upgrade.GetInstanceID(), GameEventsEnum.Upgrade);
    }

    public void AddWeight(FoodShopItem item)
    {
        Gold -= item.RelativePrice(GoldCostModifier);

        var weightToAdd = Mathf.Min(Mathf.FloorToInt(item.Weight * WeightModifier), GameConstants.MaxAnimalWeight - Animal.AnimalWeight);
        Animal.AnimalWeight += weightToAdd;
        TotalWeightAcquired += weightToAdd;
        TotalFoodBought++;
    }

    public void AddEnemy(Enemy enemy)
    {
        CurrentEnemies.Add(enemy);
    }

    public void AddEnemy(List<Enemy> enemies)
    {
        foreach (var enemy in enemies)
            AddEnemy(enemy);
    }

    public void ResetEnemies()
    {
        foreach (var enemy in CurrentEnemies)
        {
            enemy.gameObject.SetActive(false);
        }

        CurrentEnemies.Clear();
    }

    public void AddLoot(LootItem item)
    {
        Loot.Add(item);
        CalculateLootModifiers();
    }

    public void ResetData()
    {
        ScriptableObjectUtils.Reset(this);
    }

    private void CalculateLootModifiers()
    {
        Animal.Damage = GameConstants.StartingAnimalDamage + Loot.Sum(x => x.Damage);
        Animal.Speed  = GameConstants.StartingAnimalSpeed + Loot.Sum(x => x.Speed);
        Animal.Armor  = GameConstants.StartingAnimalArmor + Loot.Sum(x => x.Armor);
        Animal.CritChance = GameConstants.StartingAnimalCritChance + Loot.Sum(x => x.CritChance);
        Animal.CritDamage = GameConstants.StartingAnimalCritDamage + Loot.Sum(x => x.CritDamage);
    }

    #endregion
}