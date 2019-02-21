using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(UIShopRowTextFields))]
public class UIShopRow : MonoBehaviour, IIntializeInactive
{
    public ShopItem item;

    protected UIShopRowTextFields fields;
    protected Button buyButton;    

    protected int cachedGoldAmount;
    protected float cachedCostModifier;

    protected bool isBuyingDisabled;
    

    public virtual void ForceAwake()
    {
        fields = GetComponent<UIShopRowTextFields>();
        buyButton = GetComponentInChildren<Button>(true);
        buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    public virtual void ForceStart()
    {
        Events.Register<int>(GameEventsEnum.DataGoldChanged, OnGoldAmountChanged);
        Events.Register<float>(GameEventsEnum.DataGoldCostModifier, OnGoldCostModifierChanged);
    }

    protected virtual void OnValidate()
    {
        if (item == null)
            return;

        UpdateUI();
    }

    private void OnGoldAmountChanged(int gold)
    {
        cachedGoldAmount = gold;
        UpdateInteractable();
    }

    

    protected virtual void UpdateInteractable()
    {
        bool interactable = !isBuyingDisabled;

        if (cachedGoldAmount < item.RelativePrice(cachedCostModifier))
            interactable = false;

        buyButton.interactable = interactable;
    }

    private void OnGoldCostModifierChanged(float costModifier)
    {
        UpdateUI(costModifier);
    }       

    protected virtual void UpdateUI(float costModifier = 1f)
    {
        if (isBuyingDisabled)
            return;

        cachedCostModifier = costModifier;

        fields.Icon.sprite = item.Icon;
        fields.titleText.text = item.Name;
        fields.descriptionText.text = item.GetDescription();
        fields.costText.text = item.RelativePrice(costModifier).ToString();
    }

    protected virtual void OnBuyButtonClick()
    {

    }
}
