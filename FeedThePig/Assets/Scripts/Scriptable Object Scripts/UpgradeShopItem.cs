using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeShopItem", menuName = "Scriptables/ShopItem/UpgradeShopItem")]
public class UpgradeShopItem : ShopItem
{
    public UpgradeTypeEnum UpgradeType;
    public float Value;
    
    private void OnEnable()
    {
        PurchaseType = ShopPurchaseTypeEnum.Upgrade;        
    }

    public override string GetDescription()
    {
        return Description.Replace("{0}", Value.ToString());
    }
}
