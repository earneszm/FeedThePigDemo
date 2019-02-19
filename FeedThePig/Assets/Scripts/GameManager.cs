using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }


    // Data
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private AnimalController animalObject;

    private EffectsManager effectsManager;

    // Systems Controllers
    private TimeController timeController;
    private SpawnController spawnController;

    private bool IsInitializeDone;
    

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
        spawnController = GetComponent<SpawnController>();
        timeController = new TimeController(gameData, DateTime.Now);
        timeController.TogglePause();
        StartCoroutine(LoadDataFromFile());
    }

    private void Update()
    {
        if (IsInitializeDone == false)
            return;

        timeController.DoUpdate(Time.deltaTime);

        spawnController.MoveEnemies(animalObject.transform);

        if (gameData.IsPlayerMoving == true)
            spawnController.Spawn(gameData.PlayerDistance, animalObject);
    }

    private void OnApplicationQuit()
    {
        gameData.OnApplicationQuit();
        SaveController.Save(gameData);
    }

    private IEnumerator LoadDataFromFile()
    {
        yield return new WaitForEndOfFrame();

        // SaveController.Load(gameData);
        ScriptableObjectUtils.Reset(gameData);

        gameData.OnApplicationOpen();

       // if (gameData.IsExistingUser)
       //     ShowWelcomeBackData();

        
        animalObject.Initialize(gameData.Animal);

        gameData.CurrentLevel = 1;
        StartLevel(gameData.CurrentLevel);

        timeController.TogglePause(false);

        IsInitializeDone = true;
    }

    private void StartLevel(int levelNumber)
    {
        spawnController.LoadSpawner(levelNumber);
        effectsManager.SetOverlayText("Level " + levelNumber);
    }

    private void ShowWelcomeBackData()
    {
        var dateDifference = DateTime.Now - gameData.LastAppClosedTime;
        var goldEarned = Mathf.Min(gameData.MaxGoldPerOfflinePeriod, Mathf.FloorToInt((float)(dateDifference.TotalMinutes * gameData.GoldRatePerMinute)));

        var maxWeightDeltaAllowed = Mathf.Max(0, GameConstants.MaxAnimalWeight - gameData.Animal.AnimalWeight);
        var weightGained = Mathf.Min(gameData.MaxWeightPerOfflinePeriod, (float)(dateDifference.TotalMinutes * gameData.WeightRatePerMinute));
        weightGained = Mathf.Min(maxWeightDeltaAllowed, weightGained);

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

    public void OnPlayerMovementUpdate(bool isMoving, float speed)
    {
        gameData.IsPlayerMoving = isMoving;
        gameData.PlayerDistance += speed * Time.deltaTime;
    }

    public void OnShopItemPurchased(ShopItem item, Transform startingLocation)
    {
        // sanity check to make sure we can actually do this, if not we will fire the change event again
        if (gameData.Gold < item.RelativePrice(gameData.GoldCostModifier))
        {
            gameData.Gold += 0;
            return;
        }

        if (item is FoodShopItem && gameData.Animal.CanAddWeight)
            gameData.AddWeight(item as FoodShopItem);
        else if (item is UpgradeShopItem)
            UpgradeController.HandleUpgrade(gameData, item as UpgradeShopItem);
        else
            return;

        effectsManager.LaunchItem(startingLocation, item.Icon);
    }

    public void OnSellAnimal()
    {
        var goldAmountFromSale = Mathf.FloorToInt(gameData.Animal.AnimalWeight * gameData.goldPerWeightPrice);
        var saleWeight = gameData.Animal.AnimalWeight;

        gameData.Gold += goldAmountFromSale;
        gameData.AnimalsSold++;
        gameData.Animal.AnimalWeight = 100f;

        gameData.ResetEnemies();
        UIManager.Instance.OpenDialog(DialogTypeEnum.AnimalSale, saleWeight.ToString("N2"), goldAmountFromSale.ToString());
     //   gameData.AddEnemy(spawnController.SpawnLevel(1));
    }

    public void OnPauseableMenuToggled(bool isOpened)
    {
        timeController.TogglePause(isOpened);
    }

    public void GainGold(int goldAmount, bool isAnimate = true)
    {
        gameData.AddGold(goldAmount);
    }

    public void AdvanceLevel()
    {
        gameData.CurrentLevel++;
        StartLevel(gameData.CurrentLevel);
    }

    public void CreateLoot(LootItem item)
    {
        effectsManager.SetOverlayText(string.Format("Found item: {0}", item.Description), 2f);
    }

    public void ResetGame()
    {
        gameData.ResetData();
        gameData.ForceDataBind();
        spawnController.Reset();
    }

    #endregion
}

public class UpgradeController
{
    public static void HandleUpgrade(GameData data, UpgradeShopItem upgrade)
    {
        switch (upgrade.UpgradeType)
        {
            case UpgradeTypeEnum.GoldProductionRate:
                data.GoldRatePerMinute += upgrade.Value;
                break;            
            case UpgradeTypeEnum.WeightProductionRate:
                data.WeightRatePerMinute += upgrade.Value;
                break;
            case UpgradeTypeEnum.NotSet:
            case UpgradeTypeEnum.MaxOfflineWeightAmount:
            case UpgradeTypeEnum.MaxOfflineGoldAmount:
            default:
                Debug.LogError("Upgrade not implemented: " + nameof(upgrade.UpgradeType));
                return;
        }

        data.AddUpgrade(upgrade);
    }
}
