using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot", menuName = "Scriptables/Loot")]
public class LootItem : ScriptableObject
{
    public string Description;
    public Sprite Icon;    
    public float DropWeight = 50f;
    public LaunchTargetLocationEnum onPickupGoTo;
}
