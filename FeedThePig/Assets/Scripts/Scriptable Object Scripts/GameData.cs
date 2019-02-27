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
    public int currentLevel;   
    public Animal Animal;    

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
    public int   Gold                  { get { return gold;                } set { gold = value;                Events.Raise(gold,                GameEventsEnum.DataGoldChanged);         } }        
    public int   AnimalsSold           { get { return animalsSold;         } set { animalsSold = value;         Events.Raise(animalsSold,         GameEventsEnum.DataAnimalSoldChanged);   } }    
    public int   TotalFoodBought       { get { return totalFoodBought;     } set { totalFoodBought = value;     Events.Raise(totalFoodBought,     GameEventsEnum.DataTotalFoodBought);     } }
    public float TotalWeightAcquired   { get { return totalWeightAcquired; } set { totalWeightAcquired = value; Events.Raise(totalWeightAcquired, GameEventsEnum.DataTotalWeightAcquired); } }
    public float GoldCostModifier      { get { return goldCostModifier;    } set { goldCostModifier = value;    Events.Raise(goldCostModifier,    GameEventsEnum.DataGoldCostModifier);    } }
    public float WeightModifier        { get { return weightModifier;      } set { weightModifier = value;      Events.Raise(weightModifier,      GameEventsEnum.DataWeightModifier);      } }
    public float GoldRatePerMinute     { get { return goldRatePerMinute;   } set { goldRatePerMinute = value;   Events.Raise(goldRatePerMinute,   GameEventsEnum.DataGoldProduction);      } }
    public float WeightRatePerMinute   { get { return weightRatePerMinute; } set { weightRatePerMinute = value; Events.Raise(weightRatePerMinute, GameEventsEnum.DataWeightProduction);    } }

    public int   CurrentLevel          { get { return currentLevel;        } set { currentLevel = value;        Events.Raise(currentLevel, GameEventsEnum.EventLoadLevel); } }

    #endregion

    #region Helper Methods

    // Public Helpers

    public void UpdateAnimal(Animal animal)
    {
        Animal = animal;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }

    public void OnApplicationOpen()
    {
        LastLoginTime = DateTime.Now;
        ForceDataBind();
        RegisterForEvents();
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
            Events.Raise(upgradeID, GameEventsEnum.EventUpgrade);
        }
    }

    public void AddUpgrade(UpgradeShopItem upgrade)
    {        
        PurchasedUpgradeIDs.Add(upgrade.GetInstanceID());

        Events.Raise(upgrade.GetInstanceID(), GameEventsEnum.EventUpgrade);
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
            enemy.ForceReset();
        }

        CurrentEnemies.Clear();
    }

    public void ResetData()
    {
        ScriptableObjectUtils.Reset(this);
    }

    private void ResetLevelData()
    {
        Gold = GameConstants.StartingGold;

        Loot.Clear();
        ResetEnemies();
        Animal.ResetData();
    }

    public void ShowWelcomeBackData()
    {
        var dateDifference = DateTime.Now - LastAppClosedTime;
        var goldEarned = Mathf.Min(MaxGoldPerOfflinePeriod, Mathf.FloorToInt((float)(dateDifference.TotalMinutes * GoldRatePerMinute)));

        var maxWeightDeltaAllowed = Mathf.Max(0, GameConstants.MaxAnimalWeight - Animal.AnimalWeight);
        var weightGained = Mathf.Min(MaxWeightPerOfflinePeriod, (float)(dateDifference.TotalMinutes * WeightRatePerMinute));
        weightGained = Mathf.Min(maxWeightDeltaAllowed, weightGained);

        Gold += goldEarned;
        Animal.AnimalWeight += weightGained;
        TotalWeightAcquired += weightGained;

        UIManager.Instance.OpenDialog(DialogTypeEnum.WelcomeBack,
            DateUtils.GetFriendlyTimeSpan(dateDifference),
            goldEarned.ToString(),
            weightGained.ToString("N2")
            );
    }

    // Private Helpers

    private void CalculateLootModifiers()
    {
        Animal.MinDamage = GameConstants.StartingAnimalMinDamage + Loot.Sum(x => x.Damage);
        Animal.MaxDamage = GameConstants.StartingAnimalMaxDamage + Loot.Sum(x => x.Damage);
        Animal.Speed  = GameConstants.StartingAnimalSpeed + Loot.Sum(x => x.Speed);
        Animal.Armor  = GameConstants.StartingAnimalArmor + Loot.Sum(x => x.Armor);
        Animal.CritChance = GameConstants.StartingAnimalCritChance + Loot.Sum(x => x.CritChance);
        Animal.CritDamage = GameConstants.StartingAnimalCritDamage + Loot.Sum(x => x.CritDamage);
    }

    private void HandleUpgrade(UpgradeShopItem upgrade)
    {
        switch (upgrade.UpgradeType)
        {
            case UpgradeTypeEnum.GoldProductionRate:
                GoldRatePerMinute += upgrade.Value;
                break;
            case UpgradeTypeEnum.WeightProductionRate:
                WeightRatePerMinute += upgrade.Value;
                break;
            case UpgradeTypeEnum.DamageModified:
                Animal.MinDamage += (int)upgrade.Value;
                Animal.MaxDamage += (int)upgrade.Value;
                break;
            case UpgradeTypeEnum.NotSet:
            case UpgradeTypeEnum.MaxOfflineWeightAmount:
            case UpgradeTypeEnum.MaxOfflineGoldAmount:
            default:
                Debug.LogError("Upgrade not implemented: " + nameof(upgrade.UpgradeType));
                return;
        }

        AddUpgrade(upgrade);
    }

    private void GainLoot(LootItem item)
    {
        Loot.Add(item);
        CalculateLootModifiers();
    }

    

    #endregion

    #region Events

    private void RegisterForEvents()
    {
              
        Events.Register<ShopItem, Transform>(GameEventsEnum.EventShopItemPurchased, OnShopItemPurchased);
        Events.Register(GameEventsEnum.EventAnimalSold, OnSellAnimal);

        Events.Register<LootItem>(GameEventsEnum.EventLootGained,  GainLoot);
        Events.Register<int>(GameEventsEnum.EventGoldGained,     (amount) => { Events.StartCoroutine(Utils.IncrementOverTime(amount, AddGold)); });
        Events.Register(GameEventsEnum.EventAnimalDeath,               () => { ResetLevelData(); CurrentLevel = 1; });
        Events.Register(GameEventsEnum.EventAdvanceLevel,              () => { CurrentLevel++; });
        Events.Register(GameEventsEnum.EventGameRestart,               () => { ResetLevelData(); CurrentLevel = 1; });
    }

    private void OnShopItemPurchased(ShopItem item, Transform startingLocation)
    {
        // sanity check to make sure we can actually do this, if not we will fire the change event again
        if (Gold < item.RelativePrice(GoldCostModifier))
        {
            Gold += 0;
            return;
        }

        if (item is FoodShopItem && Animal.CanAddWeight)
            AddWeight(item as FoodShopItem);
        else if (item is UpgradeShopItem)
            HandleUpgrade(item as UpgradeShopItem);
        else
            return;
    }

    private void OnSellAnimal()
    {
        var goldAmountFromSale = Mathf.FloorToInt(Animal.AnimalWeight * goldPerWeightPrice);
        var saleWeight = Animal.AnimalWeight;

        AddGold(goldAmountFromSale);
        AnimalsSold++;
        Animal.AnimalWeight = 100f;

        ResetEnemies();
        UIManager.Instance.OpenDialog(DialogTypeEnum.AnimalSale, saleWeight.ToString("N2"), goldAmountFromSale.ToString());
    }

    

    #endregion
}