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
