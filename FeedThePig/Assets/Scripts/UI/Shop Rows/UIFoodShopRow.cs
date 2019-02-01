using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIFoodShopRow : UIShopRow
{
    [SerializeField]
    private TextMeshProUGUI weightText;

    private float cachedWeightAmount;
    private float cachedWeightModifier;

    protected override void OnValidate()
    {
        if (item == null)
            return;

        UpdateUI();
    }

    public override void ForceStart()
    {
        Events.Register<float>(GameEventsEnum.AnimalWeight, OnAnimalWeightChanged);
        Events.Register<float>(GameEventsEnum.WeightModifier, OnWeightModifierChanged);

        base.ForceStart();
    }

    protected override void UpdateInteractable()
    {
        bool interactable = cachedWeightAmount < GameConstants.MaxAnimalWeight && !isBuyingDisabled;

        if (cachedGoldAmount < item.RelativePrice(cachedCostModifier))
            interactable = false;

        buyButton.interactable = interactable;
    }

    private void OnAnimalWeightChanged(float weight)
    {
        cachedWeightAmount = weight;
        UpdateInteractable();
    }

    private void OnWeightModifierChanged(float weightModifier)
    {
        UpdateUI(cachedCostModifier, weightModifier);
    }

    private void UpdateUI(float costModifier = 1f, float weightModifier = 1f)
    {
        cachedWeightModifier = weightModifier;
        
        weightText.text = string.Format("{0} lbs.", Mathf.FloorToInt(((FoodShopItem)item).Weight * weightModifier).ToString());

        base.UpdateUI(costModifier);
    }

    protected override void OnBuyButtonClick()
    {
        GameManager.Instance.OnShopItemPurchased(item, fields.Icon.transform);
    }
}
