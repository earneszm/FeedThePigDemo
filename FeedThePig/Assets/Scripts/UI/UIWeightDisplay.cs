using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIWeightDisplay : MonoBehaviour
{
    private TextMeshProUGUI weightText;

    private void Awake()
    {
        weightText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        //   GameManager.Instance.OnAnimalWeightChanged += OnAnimalWeightChanged;
        Events.Register<float>(GameEventsEnum.AnimalWeight, OnAnimalWeightChanged);
    }

    private void OnAnimalWeightChanged(float amount)
    {
        weightText.text = amount.ToString("N1");
    }
}
