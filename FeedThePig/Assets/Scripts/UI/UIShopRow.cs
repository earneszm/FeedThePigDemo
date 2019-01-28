using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIShopRow : MonoBehaviour, IIntializeInactive
{
    public ShopItem item;

    [SerializeField]
    private Image Icon;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField]
    private TextMeshProUGUI costText;
    [SerializeField]
    private TextMeshProUGUI weightText;

    private Button buyButton;

    private float cachedCostModifier;

    public void ForceAwake()
    {
        buyButton = GetComponentInChildren<Button>(true);
        buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    public void ForceStart()
    {
        GameManager.Instance.OnGoldAmountChanged += OnGoldAmountChanged;
        GameManager.Instance.OnFoodModiferChanged += OnFoodModifierChanged;
    }

    

    private void OnValidate()
    {
        if (item == null)
            return;

        UpdateUI();
    }

    private void OnGoldAmountChanged(int gold)
    {
        buyButton.interactable = gold < item.RelativePrice(cachedCostModifier) ? false : true;
    }

    private void OnFoodModifierChanged(float costModifier, float weightModifier)
    {
        UpdateUI(costModifier, weightModifier);
    }

    private void UpdateUI(float costModifier = 1f, float weightModifier = 1f)
    {
        cachedCostModifier = costModifier;

        Icon.sprite = item.Icon;
        titleText.text = item.name;
        descriptionText.text = item.Description;
        costText.text = item.RelativePrice(costModifier).ToString();
        weightText.text = string.Format("{0} lbs.", Mathf.FloorToInt(item.Weight * weightModifier).ToString());
    }

    private void OnBuyButtonClick()
    {
        GameManager.Instance.OnShopItemPurchased(item, Icon.transform);
    }
}
