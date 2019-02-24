using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Animal
{
    public AnimalTypesEnum animalType;

    public float PlayerDistance;
    public bool IsPlayerMoving;

    [SerializeField]
    private float animalWeight = GameConstants.StartingAnimalWeight;
    public float AnimalWeight
    {
        get { return animalWeight; }
        set {
            if (value <= 0)
                animalWeight = 0;
            else
                animalWeight = value;

            Events.Raise(animalWeight, GameEventsEnum.DataAnimalWeightChanged);

            if (animalWeight <= 0)
            {
                Events.Raise(GameEventsEnum.EventAnimalDeath);
            }
        }        
    }

    public int Damage
    {
        get { return UnityEngine.Random.Range(MinDamage, MaxDamage); }
    }

    [SerializeField]
    private int minDamage = GameConstants.StartingAnimalMinDamage;
    public int MinDamage
    {
        get { return minDamage; }
        set
        {
            minDamage = value;
            Events.Raise(minDamage, MaxDamage, GameEventsEnum.DataAnimalDamageChanged);
        }
    }

    [SerializeField]
    private int maxDamage = GameConstants.StartingAnimalMaxDamage;
    public int MaxDamage
    {
        get { return maxDamage; }
        set
        {
            maxDamage = value;
            Events.Raise(MinDamage, maxDamage, GameEventsEnum.DataAnimalDamageChanged);
        }
    }

    [SerializeField]
    private int armor = GameConstants.StartingAnimalArmor;
    public int Armor
    {
        get { return armor; }
        set
        {
            armor = value;
            Events.Raise(armor, GameEventsEnum.DataAnimalArmorChanged);
        }
    }

    [SerializeField]
    private float speed = GameConstants.StartingAnimalSpeed;
    public float Speed
    {
        get { return speed; }
        set
        {
            speed = value;
            Events.Raise(speed, GameEventsEnum.DataAnimalSpeedChanged);
        }
    }

    [SerializeField]
    private float critChance = GameConstants.StartingAnimalCritChance;
    public float CritChance
    {
        get { return critChance; }
        set
        {
            critChance = value;
            Events.Raise(critChance, GameEventsEnum.DataAnimalCritChanceChanged);
        }
    }

    [SerializeField]
    private float critDamage = GameConstants.StartingAnimalCritDamage;
    public float CritDamage
    {
        get { return critDamage; }
        set
        {
            critDamage = value;
            Events.Raise(critDamage, GameEventsEnum.DataAnimalCritDamageChanged);
        }
    }

    public bool CanAddWeight { get { return AnimalWeight < GameConstants.MaxAnimalWeight; } }

    public void ResetData()
    {
        AnimalWeight = GameConstants.StartingAnimalWeight;
        MinDamage    = GameConstants.StartingAnimalMinDamage;
        MaxDamage    = GameConstants.StartingAnimalMaxDamage;
        Armor        = GameConstants.StartingAnimalArmor;
        Speed        = GameConstants.StartingAnimalSpeed;
        CritChance   = GameConstants.StartingAnimalCritChance;
        CritDamage   = GameConstants.StartingAnimalCritDamage;
    }

    public void ForceDataBind()
    {
        AnimalWeight += 0;
        MinDamage += 0; 
        Armor += 0;
        Speed += 0;
        CritChance += 0;
        CritDamage += 0;
    }

    public int RollDamage()
    {
        if (UnityEngine.Random.Range(1, 100) <= CritChance)
            return Mathf.FloorToInt(Damage * (1 + (CritDamage / 100)));

        return Damage;
    }
}
