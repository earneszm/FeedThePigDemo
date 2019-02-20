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
        Events.Register<int>(GameEventsEnum.AnimalDamageChanged, OnDamageChanged);
        Events.Register<int>(GameEventsEnum.AnimalArmorChanged, OnArmorChanged);
        Events.Register<float>(GameEventsEnum.AnimalSpeedChanged, OnSpeedChanged);
        Events.Register<float>(GameEventsEnum.AnimalCritChanceChanged, OnCritChanceChanged);
        Events.Register<float>(GameEventsEnum.AnimalCritDamageChanged, OnCritDamageChanged);

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