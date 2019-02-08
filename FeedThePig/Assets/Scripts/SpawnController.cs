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
    

    public List<Enemy> SpawnLevel(int levelNumber)
    {
        var spawner = spawners.SingleOrDefault(x => x.LevelNumber == levelNumber);

        if (spawner == null)
            Debug.LogError("Level not found: " + levelNumber);

        return spawner.Spawn(spawnPoolLocation);
    }
}
