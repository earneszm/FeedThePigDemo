using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHomeContent : MonoBehaviour, IIntializeInactive
{
    [SerializeField]
    private UIDynamicText goldPerMinute;
    [SerializeField]
    private UIDynamicText animalsSold;
    [SerializeField]
    private UIDynamicText foodItemsBought;
    [SerializeField]
    private UIDynamicText totalWeightOfFood;


    public void ForceAwake()
    {
    }

    public void ForceStart()
    {
        Events.Register<float>(GameEventsEnum.GoldProduction, OnGoldProductionChanged);
        Events.Register<int>(GameEventsEnum.AnimalSold, OnAnimalSoldChanged);
        Events.Register<int>(GameEventsEnum.TotalFoodBought, OnTotalFoodBought);
        Events.Register<float>(GameEventsEnum.TotalWeightAcquired, OnTotalWeightAcquired);
        
    }

    private void OnGoldProductionChanged (float amount)
    {
        goldPerMinute.UpdateContent(amount);
    }

    private void OnAnimalSoldChanged (int number)
    {
        animalsSold.UpdateContent(number);
    }

    private void OnTotalFoodBought(int number)
    {
        foodItemsBought.UpdateContent(number);
    }

    private void OnTotalWeightAcquired(float number)
    {
        totalWeightOfFood.UpdateContent(number.ToString("N2"));
    }
}
