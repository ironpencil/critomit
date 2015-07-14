using UnityEngine;
using System.Collections;

public class LevelSpawner : MonoBehaviour {
    
    public float lastSpawn = 0.0f;

    public SpawnManager.EnemyType enemyType;

	// Use this for initialization
	void Start () {
        bool spawnOnStart = SpawnManager.Instance.spawnOnStart;

        if (spawnOnStart && CanSpawn())
        {
            DoSpawnEnemy();
        }

        lastSpawn = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        float spawnTimer = SpawnManager.Instance.spawnTimer;

        if (CanSpawn())
        {
            if (Time.time >= lastSpawn + spawnTimer)
            {
                DoSpawnEnemy();

                lastSpawn = Time.time;
            }
        }
	}

    private bool CanSpawn()
    {
        return SpawnManager.Instance.spawnEnemies;
    }

    public void DoSpawnEnemy()
    {
        SpawnManager.Instance.SpawnEnemy(enemyType, gameObject.transform);
    }
}
