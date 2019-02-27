using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILootRow : PooledMonoBehaviour
{
    [SerializeField]
    private Image backdrop;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private LootItem item;
    public LootItem LootItem { get { return item; } }

    public void SetItem(LootItem item)
    {
        this.item = item;
        UpdateUI();
    }

    private void UpdateUI()
    {
        icon.sprite = item.Icon;
        icon.preserveAspect = true;
        titleText.text = item.Description;
        descriptionText.text = item.GetStatsDescription();
        backdrop.color = Utils.GetRarityColor(item.rarity);
    }
}
