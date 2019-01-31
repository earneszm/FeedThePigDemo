using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIIntializeShop : MonoBehaviour, IIntializeInactive
{
    [SerializeField]
    private GameObject shopContainer;

    [SerializeField]
    private UIShopRow shopRowPrefab;

    [SerializeField]
    private ShopPurchaseTypeEnum shopType;

    private List<UIShopRow> rows = new List<UIShopRow>();

    public void ForceAwake()
    {
        var shopItems = ScriptableObjectUtils.GetAllInstances<ShopItem>()
            .Where(x => x.PurchaseType == shopType && x.IsActive == true)
            .OrderBy(x => x.BasePrice);

        foreach (var item in shopItems)
        {
            var row = Instantiate(shopRowPrefab, shopContainer.transform);
            row.item = item;
            row.ForceAwake();
            rows.Add(row);
        }
    }

    public void ForceStart()
    {
        foreach (var row in rows)
        {
            row.ForceStart();
        }
    }
}
