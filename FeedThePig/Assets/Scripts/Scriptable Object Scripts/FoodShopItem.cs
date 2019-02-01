using UnityEngine;

[CreateAssetMenu(fileName = "FoodShopItem", menuName = "Scriptables/ShopItem/FoodShopItem")]
public class FoodShopItem : ShopItem
{    
    public int Weight;

    private void OnEnable()
    {
        PurchaseType = ShopPurchaseTypeEnum.Food;
    }
}
