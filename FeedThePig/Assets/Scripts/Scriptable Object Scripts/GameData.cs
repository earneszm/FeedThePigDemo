using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gameData", menuName = "Scriptables/gameData")]
public class GameData : ScriptableObject
{
    [SerializeField]
    private int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            GameManager.Instance.InvokeOnGoldAmountChanged(gold);
        }
    }
    private float goldRatePerSecond = 3f;

    [SerializeField]
    private float animalWeight;
    public float AnimalWeight
    {
        get { return animalWeight; }
        set
        {
            animalWeight = value;
            GameManager.Instance.InvokeOnAnimalWeightChanged(animalWeight);
        }
    }

    public float goldPerWeightPrice = 0.2f;
    public float goldCostModifier = 0.5f;
    public float weightModifier = 1.5f;

    // animal
  //  public string animalName;
    
}
