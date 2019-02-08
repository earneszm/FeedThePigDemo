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
    private int numEnemiesToSpawn = 5;
    [SerializeField]
    private float spawnSeperation = 10f;

    private void OnValidate()
    {
        var levelData = name.Replace("Spawner", "");

        if (int.TryParse(levelData, out int result))
            levelNumber = result;
        else
            Debug.LogError("Incorrect naming format for spawner: " + name);
    }

    public List<Enemy> Spawn(Transform parent)
    {
        if (enemyPrefab.GetComponent<Enemy>() == null)
            Debug.LogError("Enemy Prefab on spawner: " + name + " does not implement IEnemy");

        var enemyList = new List<Enemy>();
        for (int i = 1; i <= numEnemiesToSpawn; i++)
        {
            var go = Instantiate(enemyPrefab, new Vector3(i * spawnSeperation, 0, 0), Quaternion.identity, parent);
            enemyList.Add(go);
        }

        return enemyList;
    }
}
