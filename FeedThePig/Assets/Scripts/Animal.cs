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

    [SerializeField]
    private int damage = GameConstants.StartingAnimalDamage;
    public int Damage
    {
        get { return damage; }
        set
        {
            damage = value;
            Events.Raise(damage, GameEventsEnum.DataAnimalDamageChanged);
        }
    }

    [SerializeField]
    private int armor = GameConstants.StartingAnimalDamage;
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

    public void ForceDataBind()
    {
        AnimalWeight += 0;
        Damage += 0;
        Armor += 0;
        Speed += 0;
        CritChance += 0;
        CritDamage += 0;
    }

    public int RollDamage()
    {
        if (UnityEngine.Random.Range(1, 100) <= CritChance)
            return Mathf.FloorToInt(Damage * (1 + CritDamage));

        return Damage;
    }
}
