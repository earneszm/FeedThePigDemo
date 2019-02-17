using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoolLocation;
    [SerializeField]
    private List<Spawner> spawners;

    private Spawner currentSpawner;
    private float lastSpawnedAt;
    private float spawnThreshold = 8;

    private List<Enemy> enemies = new List<Enemy>();

    public void LoadSpawner(int levelNumber)
    {
        currentSpawner = spawners.SingleOrDefault(x => x.LevelNumber == levelNumber);

        if (currentSpawner == null)
            Debug.LogError("Level not found: " + levelNumber);
    }

    public void Spawn(float distanceTraveled)
    {
        if (distanceTraveled - lastSpawnedAt > spawnThreshold)
        {
            lastSpawnedAt = distanceTraveled;
            enemies.AddRange(currentSpawner.Spawn(spawnPoolLocation, spawnPoolLocation, 1));
        }
    }

    public void MoveEnemies(Transform target)
    {
        foreach (var enemy in enemies)
        {
            enemy.TryMove(target);
        }
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
