using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

[CreateAssetMenu(fileName = "gameData", menuName = "Scriptables/gameData")]
public class GameData : ScriptableObject, IGoldRate
{
    [SerializeField]
    private bool isExistingUser = false;
    public bool IsExistingUser { get { return isExistingUser; } private set { isExistingUser = value; } }

    [SerializeField]
    private int gold = GameConstants.StartingGold;
    public int Gold
    {
        get { return gold; }
        set { gold = value; Events.OnChange(gold, GameEventsEnum.Gold); }
    }    

    [SerializeField]
    private int animalsSold;
    public int AnimalsSold
    {
        get { return animalsSold; }
        set { animalsSold = value; Events.OnChange(animalsSold, GameEventsEnum.AnimalSold); }
    }

    [SerializeField]
    private int totalFoodBought;
    public int TotalFoodBought
    {
        get { return totalFoodBought; }
        set { totalFoodBought = value; Events.OnChange(totalFoodBought, GameEventsEnum.TotalFoodBought); }
    }

    [SerializeField]
    private float totalWeightAcquired;
    public float TotalWeightAcquired
    {
        get { return totalWeightAcquired; }
        set { totalWeightAcquired = value; Events.OnChange(totalWeightAcquired, GameEventsEnum.TotalWeightAcquired); }
    }

    public float goldPerWeightPrice = GameConstants.GoldPerWeightPrice;

    [SerializeField]
    private float goldCostModifier = GameConstants.GoldCostModifier;
    public float GoldCostModifier
    {
        get { return goldCostModifier; }
        set { goldCostModifier = value; Events.OnChange(goldCostModifier, GameEventsEnum.GoldCostModifier); }
    }

    [SerializeField]
    private float weightModifier = GameConstants.WeightModifier;
    public float WeightModifier
    {
        get { return weightModifier; }
        set { weightModifier = value; Events.OnChange(weightModifier, GameEventsEnum.WeightModifier); }
    }

    [SerializeField]
    private float goldRatePerMinute = GameConstants.GoldRatePerMinute;
    public float GoldRatePerMinute
    {
        get { return goldRatePerMinute; }
        set { goldRatePerMinute = value; Events.OnChange(goldRatePerMinute, GameEventsEnum.GoldProduction); }
    }

    [SerializeField]
    private float weightRatePerMinute = GameConstants.WeightRatePerMinute;
    public float WeightRatePerMinute
    {
        get { return weightRatePerMinute; }
        set { weightRatePerMinute = value; Events.OnChange(weightRatePerMinute, GameEventsEnum.WeightProduction); }
    }


    [SerializeField]
    private int maxGoldPerOfflinePeriod = GameConstants.MaxGoldPerOfflinePeriod;
    public int MaxGoldPerOfflinePeriod { get { return maxGoldPerOfflinePeriod; } }

    [SerializeField]
    private float maxWeightPerOfflinePeriod = GameConstants.MaxWeightPerOfflinePeriod;
    public float MaxWeightPerOfflinePeriod { get { return maxWeightPerOfflinePeriod; } }

    [SerializeField]
    private long lastLoginTime;
    public DateTime LastLoginTime { get { return new DateTime(lastLoginTime); } private set { lastLoginTime = value.Ticks; } }

    [SerializeField]
    private long lastAppClosedTime;
    public DateTime LastAppClosedTime { get { return new DateTime(lastAppClosedTime); } private set { lastAppClosedTime = value.Ticks; } }

    public Animal Animal;
    public List<int> PurchasedUpgradeIDs = new List<int>();

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

    public void ResetData()
    {
        ScriptableObjectUtils.Reset(this);
    }
}