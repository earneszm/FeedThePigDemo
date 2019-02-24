using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnController : MonoBehaviour
{
    private List<Spawner> spawners;
    private Spawner currentSpawner;
    private float lastSpawnedAt;
    private float spawnThreshold = 8;
    private int numEnemiesSpawned;

    private Vector3 spawnPosition;

    private List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        spawners = ScriptableObjectUtils.GetAllInstances<Spawner>().ToList();
    }

    private void Start()
    {
        Events.Register<int>(GameEventsEnum.EventLoadLevel, LoadSpawner);
        Events.Register(GameEventsEnum.EventAnimalDeath, Reset);

        spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 1, 0, 0));
        spawnPosition.y = 0f;
        spawnPosition.z = 0f;
    }

    public void LoadSpawner(int levelNumber)
    {
        Reset();
        currentSpawner = spawners.SingleOrDefault(x => x.LevelNumber == levelNumber);

        if (levelNumber != 1 && GameManager.Instance.IsTestMode)
            currentSpawner = null;

        if (currentSpawner == null)
            GameManager.Instance.GameOver();
        else
            Events.Raise(levelNumber, GameEventsEnum.EventStartLevel);
    }

    public bool Spawn(float distanceTraveled, ITakeDamage target)
    {
        if (currentSpawner == null || numEnemiesSpawned > currentSpawner.MaxEnemiesToSpawn)
            return true;

        if (distanceTraveled - lastSpawnedAt > spawnThreshold)
        {
            lastSpawnedAt = distanceTraveled;
            enemies.AddRange(currentSpawner.Spawn(spawnPosition, target, 1, numEnemiesSpawned == currentSpawner.MaxEnemiesToSpawn));
            numEnemiesSpawned++;
        }

        return false;
    }

    public void MoveEnemies(Transform target, float speed)
    {
        foreach (var enemy in enemies)
        {
            enemy.TryMove(target, speed);
        }
    }

    public void Reset()
    {
        lastSpawnedAt = 0;
        numEnemiesSpawned = 0;

        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }

        enemies.Clear();
    }
}
