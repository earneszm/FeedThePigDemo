using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Animal
{
    public AnimalTypesEnum animalType;

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

            Events.OnChange(animalWeight, GameEventsEnum.AnimalWeight);

            if (animalWeight <= 0)
            {
                Events.OnChange(animalWeight, GameEventsEnum.AnimalDeath);
                GameManager.Instance.ResetGame();
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
            Events.OnChange(damage, GameEventsEnum.AnimalDamageChanged);
        }
    }

    public bool CanAddWeight { get { return AnimalWeight < GameConstants.MaxAnimalWeight; } }

    public void ForceDataBind()
    {
        AnimalWeight += 0;
    }
}
