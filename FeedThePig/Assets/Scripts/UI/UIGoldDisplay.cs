using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIGoldDisplay : MonoBehaviour
{
    private TextMeshProUGUI goldTexxt;

    private void Awake()
    {
        goldTexxt = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Instance.OnGoldAmountChanged += OnGoldAmountChanged;
    }

    private void OnGoldAmountChanged(int amount)
    {
        goldTexxt.text = amount.ToString();
    }
}
