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

        gameData.Gold += goldEarned;

        UIManager.Instance.OpenDialog(DialogTypeEnum.WelcomeBack,
            DateUtils.GetFriendlyTimeSpan(dateDifference),
            goldEarned.ToString()
            );
    }

    #region Events

    public void OnShopItemPurchased(ShopItem item, Transform startingLocation)
    {
        if (gameData.Gold >= item.RelativePrice(gameData.GoldCostModifier))
        {
            gameData.Gold -= item.RelativePrice(gameData.GoldCostModifier);
            gameData.AnimalWeight += Mathf.FloorToInt(item.Weight * gameData.WeightModifier);
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
