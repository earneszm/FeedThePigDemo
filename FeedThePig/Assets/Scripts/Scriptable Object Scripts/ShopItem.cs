using System;
using UnityEngine;

public abstract class ShopItem : ScriptableObject, IShopItemPurchaseType
{
    [HideInInspector]
    public virtual ShopPurchaseTypeEnum PurchaseType { get; protected set; }

    public string Name;
    [SerializeField]
    protected string Description;
    public int BasePrice;
    public Sprite Icon;
    public bool IsActive;    

    public int RelativePrice(float costModifier = 1f)
    {
        return Mathf.FloorToInt(BasePrice * costModifier);
    }

    public virtual string GetDescription()
    {
        return Description;
    }
}

[CreateAssetMenu(fileName = "FoodShopItem", menuName = "Scriptables/ShopItem/FoodShopItem")]
public class FoodShopItem : ShopItem
{    
    public int Weight;

    private void OnEnable()
    {
        PurchaseType = ShopPurchaseTypeEnum.Food;
    }
}

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
