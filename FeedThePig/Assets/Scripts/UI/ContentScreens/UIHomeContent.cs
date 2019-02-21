using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHomeContent : MonoBehaviour, IIntializeInactive
{
    [SerializeField]
    private UIDynamicText damage;
    [SerializeField]
    private UIDynamicText armor;
    [SerializeField]
    private UIDynamicText speed;
    [SerializeField]
    private UIDynamicText critChance;
    [SerializeField]
    private UIDynamicText critDamage;


    public void ForceAwake()
    {
    }

    public void ForceStart()
    {
        Events.Register<int>(GameEventsEnum.DataAnimalDamageChanged, OnDamageChanged);
        Events.Register<int>(GameEventsEnum.DataAnimalArmorChanged, OnArmorChanged);
        Events.Register<float>(GameEventsEnum.DataAnimalSpeedChanged, OnSpeedChanged);
        Events.Register<float>(GameEventsEnum.DataAnimalCritChanceChanged, OnCritChanceChanged);
        Events.Register<float>(GameEventsEnum.DataAnimalCritDamageChanged, OnCritDamageChanged);

    }

    private void OnDamageChanged(int amount)
    {
        damage.UpdateContent(amount);
    }

    private void OnArmorChanged(int number)
    {
        armor.UpdateContent(number);
    }

    private void OnSpeedChanged(float number)
    {
        speed.UpdateContent(number);
    }

    private void OnCritChanceChanged(float number)
    {
        critChance.UpdateContent(number.ToString("N2"));
    }

    private void OnCritDamageChanged(float number)
    {
        critDamage.UpdateContent(number.ToString("N2"));
    }
}