using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [SerializeField]
    private Transform spawnLocation;

    // UI Effects Stuff
    [SerializeField]
    private GameObject flyingItemPrefab;
    [SerializeField]
    private RectTransform feedTarget;
    [SerializeField]
    private GameObject effectsCanvas;


    // Data
    [SerializeField]
    private GameData gameData;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        SetStartingData();
        StartCoroutine(PushOutStartingData());
    }

    private void SetStartingData()
    {
        gameData.Gold = 55;
        gameData.AnimalWeight = 100f;
    }

    private IEnumerator PushOutStartingData()
    {
        yield return new WaitForEndOfFrame();

        InvokeOnFoodModiferChanged(gameData.goldCostModifier, gameData.weightModifier);
        InvokeOnGoldAmountChanged(gameData.Gold);
        InvokeOnAnimalWeightChanged(gameData.AnimalWeight);
    }

    #region Events

    public Action<int> OnGoldAmountChanged;
    public void InvokeOnGoldAmountChanged(int amount)
    {
        if (OnGoldAmountChanged != null)
            OnGoldAmountChanged.Invoke(amount);
    }

    public Action<float> OnAnimalWeightChanged;
    public void InvokeOnAnimalWeightChanged(float animalWeight)
    {
        if (OnAnimalWeightChanged != null)
            OnAnimalWeightChanged.Invoke(animalWeight);
    }

    public Action<float, float> OnFoodModiferChanged;
    public void InvokeOnFoodModiferChanged(float costModifier, float weightModifier)
    {
        if (OnFoodModiferChanged != null)
            OnFoodModiferChanged.Invoke(costModifier, weightModifier);
    }

    public void OnShopItemPurchased(ShopItem item, Transform startingLocation)
    {
        if (gameData.Gold >= item.RelativePrice(gameData.goldCostModifier))
        {
            gameData.Gold -= item.RelativePrice(gameData.goldCostModifier);
            gameData.AnimalWeight += Mathf.FloorToInt(item.Weight * gameData.weightModifier);
        }
        else // sanity check to make sure we can actually do this, if not we will fire the change event again
        {
            gameData.Gold += 0;
            return;
        }

        // animate icon going to animal
        var newEffect = Instantiate(flyingItemPrefab, startingLocation.transform.position, Quaternion.identity, effectsCanvas.transform);
        var move = newEffect.GetComponent<MoveToTargetAndFade>();
        newEffect.GetComponentInChildren<Image>().sprite = item.Icon;
        move.target = feedTarget;
        move.StartMoving();
    }

    public void OnSellAnimal()
    {
        var goldAmountFromSale = Mathf.FloorToInt(gameData.AnimalWeight * gameData.goldPerWeightPrice);
        var saleWeight = gameData.AnimalWeight;

        gameData.Gold += goldAmountFromSale;
        gameData.AnimalWeight = 100f;

        UIManager.Instance.OpenDialog(DialogTypeEnum.AnimalSale, saleWeight.ToString(), goldAmountFromSale.ToString());
    }

    #endregion
}
