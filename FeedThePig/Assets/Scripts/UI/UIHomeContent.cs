using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHomeContent : MonoBehaviour, IIntializeInactive
{
    [SerializeField]
    private UIDynamicText goldPerMinute;

    public void ForceAwake()
    {
    }

    public void ForceStart()
    {
        Events.Register<float>(GameEventsEnum.GoldProduction, OnGoldProductionChanged);
    }

    private void OnGoldProductionChanged (float amount)
    {
        goldPerMinute.UpdateContent(amount);
    }
}
