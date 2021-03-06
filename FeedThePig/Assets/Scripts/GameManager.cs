﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

[RequireComponent(typeof(EffectsManager))]
[RequireComponent(typeof(SpawnController))]
[RequireComponent(typeof(DroppableController))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    private bool IsInitializeDone;

    // Testing
    public bool IsTestMode;

    // Data
    public Config Config;
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private AnimalController animalObject;
    [SerializeField]
    private GameObject testObject;
    [SerializeField]
    private RectTransform testSpot;


    // Systems Controllers
    private TimeController timeController;
    private EffectsManager effectsManager;
    private SpawnController spawnController;
    private DroppableController droppableController;

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
        droppableController = GetComponent<DroppableController>();

        timeController = new TimeController(gameData, DateTime.Now);
        timeController.TogglePause();

        StartCoroutine(LoadDataFromFile());
    }

    private void Update()
    {
        if (IsInitializeDone == false)
            return;

        timeController.DoUpdate(Time.deltaTime);

        spawnController.MoveEnemies(animalObject.transform, gameData.Animal.Speed);

        if (gameData.Animal.IsPlayerMoving == true)
            spawnController.Spawn(gameData.Animal.PlayerDistance, animalObject);
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
       //     gameData.ShowWelcomeBackData();
        
        animalObject.Initialize(gameData.Animal);

        gameData.CurrentLevel = 1;

        timeController.TogglePause(false);

        IsInitializeDone = true;

        // test
        //testObject.SetTarget(Camera.main.ScreenToWorldPoint(testSpot.transform.position));
        //testObject.StartMoving();
        
    }

    #region Public Methods

    public void GameOver()
    {        
        Events.Raise(GameEventsEnum.EventGameOver);
        UIManager.Instance.OpenDialog(DialogTypeEnum.GameOver);
    }

    #endregion
}
