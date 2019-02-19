using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoolLocation;
  //  [SerializeField]
    private List<Spawner> spawners;

    private Spawner currentSpawner;
    private float lastSpawnedAt;
    private float spawnThreshold = 8;
    private int numEnemiesSpawned;

    private List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        spawners = ScriptableObjectUtils.GetAllInstances<Spawner>().ToList();
    }

    public void LoadSpawner(int levelNumber)
    {
        Reset();
        currentSpawner = spawners.SingleOrDefault(x => x.LevelNumber == levelNumber);

        if (currentSpawner == null)
            Debug.LogError("Level not found: " + levelNumber);
    }

    public bool Spawn(float distanceTraveled, ITakeDamage target)
    {
        if (currentSpawner == null)
            Debug.LogError("Current spawner does not exist");

        if (numEnemiesSpawned > currentSpawner.MaxEnemiesToSpawn)
            return true;

        if (distanceTraveled - lastSpawnedAt > spawnThreshold)
        {
            lastSpawnedAt = distanceTraveled;
            enemies.AddRange(currentSpawner.Spawn(spawnPoolLocation, spawnPoolLocation, target, 1, numEnemiesSpawned == currentSpawner.MaxEnemiesToSpawn));
            numEnemiesSpawned++;
        }

        return false;
    }

    public void MoveEnemies(Transform target)
    {
        foreach (var enemy in enemies)
        {
            enemy.TryMove(target);
        }
    }

    public void Reset()
    {
        lastSpawnedAt = 0;
        numEnemiesSpawned = 0;

        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        enemies.Clear();
    }


    //
    //
    //public List<Enemy> SpawnLevel(int levelNumber)
    //{
    //    var spawner = spawners.SingleOrDefault(x => x.LevelNumber == levelNumber);
    //
    //    if (spawner == null)
    //        Debug.LogError("Level not found: " + levelNumber);
    //
    //    return spawner.Spawn(spawnPoolLocation);
    //}
}
