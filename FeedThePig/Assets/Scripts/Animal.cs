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
        set { animalWeight = value; Events.OnChange(animalWeight, GameEventsEnum.AnimalWeight); }
    }

    public void ForceDataBind()
    {
        AnimalWeight += 0;
    }
}
