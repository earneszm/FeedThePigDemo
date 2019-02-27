using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Scriptables/Config")]
public class Config : ScriptableObject
{
    [Header("Item Rarity Colors")]
    public Color CommonItemColor;
    public Color UncommonItemColor;
    public Color RareItemColor;
    public Color EpicItemColor;
    public Color LegendaryItemColor;
}
