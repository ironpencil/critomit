using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : Singleton<SpawnManager> {

    public bool spawnEnemies = false;
    public float initialSpawnTimer = 10.0f;
    public float spawnTimer = 10.0f;
    public bool spawnOnStart = true;

    public List<GameObject> smallEnemies = new List<GameObject>();
    public List<GameObject> bigEnemies = new List<GameObject>();

    public enum EnemyName
    {
        Ball,
        LilBug,
        LilFish,
        Jelly,
        Manta,
        MedFish
    }

    public GameObject ballPrefab;
    public GameObject lilBugPrefab;
    public GameObject lilFishPrefab;
    public GameObject jellyPrefab;
    public GameObject mantaPrefab;
    public GameObject medFishPrefab;
    

    [SerializeField]
    public int EnemiesRemaining
    {
        get
        {
            return EnemyObjects.transform.childCount;            
        }
    }

    [SerializeField]
    public int SpawnersRemaining
    {
        get
        {
            return SpawnerObjects.transform.childCount;
        }
    }

    public GameObject EnemyObjects;
    public GameObject SpawnerObjects;
    public IndicatorPanel IndicatorHandler;

    public enum EnemyType
    {
        Small,
        Big
    }

    // Use this for initialization
    public override void Start()
    {
        DebugLogger.Log("SpawnManager::Start()");
        destroyOnLoad = true;

        base.Start();
        spawnTimer = initialSpawnTimer;

    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyObjects == null)
        {
            EnemyObjects = GameObject.Find("Enemies");
        }

        if (SpawnerObjects == null)
        {
            SpawnerObjects = GameObject.Find("Spawners");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            spawnEnemies = !spawnEnemies;
        }
    }

    public void StartSpawners()
    {
        spawnEnemies = true;
    }

    public void StopSpawners()
    {
        spawnEnemies = false;
    }

    public GameObject GetRandomEnemy(EnemyType enemyType)
    {
        List<GameObject> enemyList;

        switch (enemyType)
        {
            case EnemyType.Small:
                enemyList = smallEnemies;
                break;
            case EnemyType.Big:
                enemyList = bigEnemies;
                break;
            default:
                return null; //invalid enemy type                
        }

        GameObject enemy = null;

        if (enemyList.Count > 0)
        {
            int enemyIndex = Random.Range(0, enemyList.Count);

            enemy = enemyList[enemyIndex];
        }

        return enemy;
    }

    public GameObject SpawnEnemy(GameObject enemyPrefab, Transform spawnLocation)
    {
        if (enemyPrefab == null) {
            DebugLogger.Log("Tried to spawn null enemy.");
            return null;
        }

        GameObject enemy = (GameObject)Instantiate(enemyPrefab, spawnLocation.position, enemyPrefab.transform.rotation);

        enemy.transform.parent = EnemyObjects.transform;

        //EnemyMovement enemyScript = enemy.GetComponent<EnemyMovement>();

        //if (enemyScript != null)
        //{
        //    enemyScript.targetRB = ObjectManager.Instance.player.GetComponent<Rigidbody2D>();
        //}

        if (IndicatorHandler != null)
        {
            IndicatorHandler.AddIndicator(enemy.transform, IndicatorPanel.IndicatorType.Enemy);
        }

        if (MutatorManager.Instance.activeMutators.Count > 0)
        {
            MutatorManager.Instance.MutateEnemy(enemy);
        }

        return enemy;
    }

    public GameObject SpawnEnemy(EnemyType enemyType, Transform spawner)
    {
        GameObject prefab = SpawnManager.Instance.GetRandomEnemy(enemyType);

        return SpawnEnemy(prefab, spawner);
    }

    public GameObject SpawnEnemy(EnemyName enemyName, Transform spawnAt)
    {
        GameObject prefab = null;

        switch (enemyName)
        {
            case EnemyName.Ball:
                prefab = ballPrefab;
                break;
            case EnemyName.LilBug:
                prefab = lilBugPrefab;
                break;
            case EnemyName.LilFish:
                prefab = lilFishPrefab;
                break;
            case EnemyName.Jelly:
                prefab = jellyPrefab;
                break;
            case EnemyName.Manta:
                prefab = mantaPrefab;
                break;
            case EnemyName.MedFish:
                prefab = medFishPrefab;
                break;
            default:
                break;
        }

        return SpawnEnemy(prefab, spawnAt);
    }

    public void ClearEnemies()
    {
        foreach (Transform child in EnemyObjects.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

}
