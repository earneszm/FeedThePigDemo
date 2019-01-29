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
    public void AddGold(int amount)
    {
        Gold += amount;
    }

    [SerializeField]
    private float animalWeight = GameConstants.StartingAnimalWeight;
    public float AnimalWeight
    {
        get { return animalWeight; }
        set { animalWeight = value; Events.OnChange(animalWeight, GameEventsEnum.AnimalWeight); }
    }

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


    public float goldPerWeightPrice = GameConstants.GoldPerWeightPrice;

    [SerializeField]
    private int maxGoldPerOfflinePeriod = GameConstants.MaxGoldPerOfflinePeriod;
    public int MaxGoldPerOfflinePeriod { get { return maxGoldPerOfflinePeriod; } }

    [SerializeField]
    private long lastLoginTime;
    public DateTime LastLoginTime { get { return new DateTime(lastLoginTime); } private set { lastLoginTime = value.Ticks; } }

    [SerializeField]
    private long lastAppClosedTime;
    public DateTime LastAppClosedTime { get { return new DateTime(lastAppClosedTime); } private set { lastAppClosedTime = value.Ticks; } }

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
        Gold += 0;
        AnimalWeight += 0;
        GoldCostModifier += 0;
        WeightModifier += 0;
        GoldRatePerMinute += 0;
    }

    public void ResetData()
    {
        var resetSource = ScriptableObject.CreateInstance<GameData>();
        if(resetSource != null)
        {
            var output = JsonUtility.ToJson(resetSource);
            JsonUtility.FromJsonOverwrite(output, this);
        }
    }
}