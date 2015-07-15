using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : Singleton<SpawnManager> {

    public bool spawnEnemies = true;
    public float spawnTimer = 5.0f;
    public bool spawnOnStart = true;

    public List<GameObject> smallEnemies = new List<GameObject>();
    public List<GameObject> bigEnemies = new List<GameObject>();

    [SerializeField]
    public int EnemiesRemaining
    {
        get
        {
            return EnemyObjects.transform.childCount;            
        }
    }

    public GameObject EnemyObjects;

    public enum EnemyType
    {
        Small,
        Big
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("SpawnManager::Start()");
        destroyOnLoad = true;

        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyObjects == null)
        {
            EnemyObjects = GameObject.Find("Enemies");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            spawnEnemies = !spawnEnemies;
        }
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
        if (enemyPrefab == null) { return null; }

        GameObject enemy = (GameObject)Instantiate(enemyPrefab, spawnLocation.position, enemyPrefab.transform.rotation);

        enemy.transform.parent = EnemyObjects.transform;

        LookAtTarget enemyScript = enemy.GetComponent<LookAtTarget>();

        if (enemyScript != null)
        {
            enemyScript.target = ObjectManager.Instance.Player.transform;
        }

        return enemy;
    }

    public GameObject SpawnEnemy(EnemyType enemyType, Transform spawner)
    {
        GameObject prefab = SpawnManager.Instance.GetRandomEnemy(enemyType);

        return SpawnEnemy(prefab, spawner);
    }

}
