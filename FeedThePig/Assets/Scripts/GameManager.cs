using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [SerializeField]
    private Transform spawnLocation;


    // Data
    [SerializeField]
    private GameData gameData;

    private EffectsManager effectsManager;

    // Systems Controllers
    private TimeController timeController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        effectsManager = GetComponent<EffectsManager>();
        timeController = new TimeController(gameData, DateTime.Now);
        StartCoroutine(LoadDataFromFile());
    }

    private void Update()
    {
        timeController.DoUpdate(Time.deltaTime);
    }

    private void OnApplicationQuit()
    {
        gameData.OnApplicationQuit();
        SaveController.Save(gameData);
    }

    private IEnumerator LoadDataFromFile()
    {
        yield return new WaitForEndOfFrame();

        SaveController.Load(gameData);

        gameData.OnApplicationOpen();

        if (gameData.IsExistingUser)
            ShowWelcomeBackData();
    }

    private void ShowWelcomeBackData()
    {
        var dateDifference = DateTime.Now - gameData.LastAppClosedTime;
        var goldEarned = Mathf.Min(gameData.MaxGoldPerOfflinePeriod, Mathf.FloorToInt((float)(dateDifference.TotalMinutes * gameData.GoldRatePerMinute)));
        var weightGained = Mathf.Min(gameData.MaxWeightPerOfflinePeriod, (float)(dateDifference.TotalMinutes * gameData.WeightRatePerMinute));

        gameData.Gold += goldEarned;
        gameData.Animal.AnimalWeight += weightGained;
        gameData.TotalWeightAcquired += weightGained;

        UIManager.Instance.OpenDialog(DialogTypeEnum.WelcomeBack,
            DateUtils.GetFriendlyTimeSpan(dateDifference),
            goldEarned.ToString(),
            weightGained.ToString("N2")
            );
    }

    #region Events

    public void OnShopItemPurchased(ShopItem item, Transform startingLocation)
    {
        if (gameData.Gold >= item.RelativePrice(gameData.GoldCostModifier))
        {
            gameData.Gold -= item.RelativePrice(gameData.GoldCostModifier);

            var weightToAdd = Mathf.Min(Mathf.FloorToInt(item.Weight * gameData.WeightModifier), GameConstants.MaxAnimalWeight);
            gameData.Animal.AnimalWeight += weightToAdd;
            gameData.TotalWeightAcquired += weightToAdd;
            gameData.TotalFoodBought++;
        }
        else // sanity check to make sure we can actually do this, if not we will fire the change event again
        {
            gameData.Gold += 0;
            return;
        }

        effectsManager.LaunchItem(startingLocation, item.Icon);
    }

    public void OnSellAnimal()
    {
        var goldAmountFromSale = Mathf.FloorToInt(gameData.Animal.AnimalWeight * gameData.goldPerWeightPrice);
        var saleWeight = gameData.Animal.AnimalWeight;

        gameData.Gold += goldAmountFromSale;
        gameData.AnimalsSold++;
        gameData.Animal.AnimalWeight = 100f;

        UIManager.Instance.OpenDialog(DialogTypeEnum.AnimalSale, saleWeight.ToString("N2"), goldAmountFromSale.ToString());
    }

    #endregion
}
