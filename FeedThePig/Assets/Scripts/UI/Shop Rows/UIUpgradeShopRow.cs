using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgradeShopRow : UIShopRow
{
    public override void ForceStart()
    {
        Events.Register<int>(GameEventsEnum.EventUpgrade, OnUpgradePurchased);
        base.ForceStart();
    }

    protected override void OnBuyButtonClick()
    {
        Events.Raise(item, fields.Icon.transform, GameEventsEnum.EventShopItemPurchased);
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
