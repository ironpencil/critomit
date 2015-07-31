using UnityEngine;
using System.Collections;

public class LevelSpawner : MonoBehaviour {
    
    public float lastSpawn = 0.0f;

    public SpawnManager.EnemyType enemyType;

    public bool spawnerActive = false;

    public bool forceOff = false;

    public bool addIndicator = true;

    public ParticleSystem particleSystem;
    public Vector2 minMaxParticles = Vector2.zero;

	// Use this for initialization
    //void Start () {
    //    bool spawnOnStart = SpawnManager.Instance.spawnOnStart;

    //    if (spawnOnStart && CanSpawn())
    //    {
    //        DoSpawnEnemy();
    //    }

    //    lastSpawn = Time.time;
    //}

    void Start()
    {
        if (addIndicator && SpawnManager.Instance.IndicatorHandler != null)
        {
            SpawnManager.Instance.IndicatorHandler.AddIndicator(transform, IndicatorPanel.IndicatorType.Spawner);
        }
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

        if (DebugLogger.DEBUG_MODE.Equals("DEBUG"))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                DoSpawnEnemy();
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
        particleSystem.Emit(Random.Range((int)minMaxParticles.x, (int)minMaxParticles.y + 1));
    }
}
