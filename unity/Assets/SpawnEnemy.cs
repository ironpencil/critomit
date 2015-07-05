using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {

    public float spawnTimer = 5.0f;
    public float lastSpawn = 0.0f;

    public GameObject enemyPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (enemyPrefab != null && Globals.Instance.SpawnEnemies)
        {
            if (Time.time >= lastSpawn + spawnTimer)
            {
                DoSpawnEnemy();

                lastSpawn = Time.time;
            }
        }
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
