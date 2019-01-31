using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgradeShopRow : UIShopRow
{
    public override void ForceStart()
    {
        Events.Register<int>(GameEventsEnum.Upgrade, OnUpgradePurchased);
        base.ForceStart();
    }

    protected override void OnBuyButtonClick()
    {
        GameManager.Instance.OnShopItemPurchased(item, fields.Icon.transform);
    }

    private void OnUpgradePurchased(int purchasedItemID)
    {
        if (isBuyingDisabled)
            return;

        if (purchasedItemID == item.GetInstanceID())
        {
            fields.costText.text = "Bought";
            isBuyingDisabled = true;
            UpdateInteractable();
        }
    }
}
