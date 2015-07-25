using UnityEngine;
using System.Collections;

public class LevelSpawner : MonoBehaviour {
    
    public float lastSpawn = 0.0f;

    public SpawnManager.EnemyType enemyType;

    public bool spawnerActive = false;

    public bool forceOff = false;

	// Use this for initialization
    //void Start () {
    //    bool spawnOnStart = SpawnManager.Instance.spawnOnStart;

    //    if (spawnOnStart && CanSpawn())
    //    {
    //        DoSpawnEnemy();
    //    }

    //    lastSpawn = Time.time;
    //}
	
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
        bool canSpawn = SpawnManager.Instance.spawnEnemies && !forceOff;

        if (canSpawn)
        {
            if (!spawnerActive)
            {
                //spawner wasn't on, so turn it on
                spawnerActive = true;
                if (SpawnManager.Instance.spawnOnStart)
                {
                    lastSpawn = -1000.0f; //spawn immediately
                }
                else
                {
                    lastSpawn = Time.time; //start spawn timer now
                }
            }
        }
        
        
        return canSpawn;
    }

    public void DoSpawnEnemy()
    {
        SpawnManager.Instance.SpawnEnemy(enemyType, gameObject.transform);
    }
}
