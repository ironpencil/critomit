using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {

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
        return enemyPrefab != null && Globals.Instance.SpawnEnemies;
    }

    public void DoSpawnEnemy()
    {
        GameObject enemy = (GameObject) Instantiate(enemyPrefab, gameObject.transform.position, enemyPrefab.transform.rotation);

        enemy.transform.parent = Globals.Instance.DynamicObjects.transform;

        LookAtTarget enemyScript = enemy.GetComponent<LookAtTarget>();

        if (enemyScript != null)
        {
            enemyScript.target = Globals.Instance.Player.transform;
        }
    }
}
