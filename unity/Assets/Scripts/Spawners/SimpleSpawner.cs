using UnityEngine;
using System.Collections;

public class SimpleSpawner : MonoBehaviour {

    public float spawnTimer = 5.0f;
    public float lastSpawn = 0.0f;
    public bool spawnOnStart = true;

    public GameObject enemyPrefab;

	// Use this for initialization
	void Start () {
        if (spawnOnStart && CanSpawn())
        {
            DoSpawnEnemy();
        }

        lastSpawn = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

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
        return enemyPrefab != null && SpawnManager.Instance.spawnEnemies;
    }

    public void DoSpawnEnemy()
    {
        SpawnManager.Instance.SpawnEnemy(enemyPrefab, gameObject.transform);
    }
}
