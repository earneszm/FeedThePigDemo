using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawner", menuName = "Scriptables/Spawner")]
public class Spawner : ScriptableObject
{
    private int levelNumber;
    public int LevelNumber { get { return levelNumber; } }

    [SerializeField]
    private Enemy enemyPrefab;
    [SerializeField]
    private Enemy bossPrefab;
   // [SerializeField]
    private int numEnemiesToSpawn = 5;
    [SerializeField]
    private int maxEnemiesToSpawn = 10;
    [SerializeField]
    private float spawnSeperation = 10f;

    public int MaxEnemiesToSpawn { get { return maxEnemiesToSpawn; } }


    private void OnValidate()
    {
        var levelData = name.Replace("Spawner", "");

        if (int.TryParse(levelData, out int result))
            levelNumber = result;
        else
            Debug.LogError("Incorrect naming format for spawner: " + name);
    }

    public List<Enemy> Spawn(Vector3 spawnLocation, ITakeDamage animalTarget, int? overrideNumToSpawn = null, bool isLastEnemy = false)
    {
        Enemy prefabToSpawn = isLastEnemy ? bossPrefab : enemyPrefab;

        if (prefabToSpawn.GetComponent<Enemy>() == null)
            Debug.LogError("Enemy Prefab on spawner: " + name + " does not implement IEnemy");

        if (overrideNumToSpawn == null)
            overrideNumToSpawn = numEnemiesToSpawn;

        var enemyList = new List<Enemy>();
        for (int i = 1; i <= overrideNumToSpawn; i++)
        {
            var go = prefabToSpawn.Get<Enemy>(spawnLocation, Quaternion.identity);
            go.Initialize(animalTarget);
            enemyList.Add(go);
        }

        return enemyList;
    }
}
