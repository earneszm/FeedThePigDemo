using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopItem")]
public class ShopItem : ScriptableObject
{
    public string Name;
    public string Description;
    public int BasePrice;
    public int Weight;
    public Sprite Icon;

    public int RelativePrice(float costModifier = 1f)
    {
        return Mathf.FloorToInt(BasePrice * costModifier);
    }
}
