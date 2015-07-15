using UnityEngine;
using System.Collections;

public class LevelManager : Singleton<LevelManager> {

    public int levelsCompleted = 0;

    public int waveLength = 10;
    public int waveLengthInc = 10;
    public float timeRemaining = 0.0f;

    public bool waveComplete = false;

    private float spawnTimeAdjustment = 0.0f;
    private bool loadingNewLevel = false;

    private float minimumSpawnTimer = 1.0f;

    void Start()
    {
        base.Start();        
        timeRemaining = waveLength;
    }

    void Update()
    {        

        if (loadingNewLevel)
        {
            Debug.LogError("!!!!!! Update called while Loading new level!!!!!!!");
        }

        timeRemaining -= Time.deltaTime;

        if (timeRemaining < 0.0f)
        {
            waveComplete = true;
            SpawnManager.Instance.spawnEnemies = false;
        }
    }

    void LateUpdate()
    {
        if (waveComplete && SpawnManager.Instance.EnemiesRemaining == 0)
        {
            Debug.Log("Level complete!");                       

            levelsCompleted++;

            spawnTimeAdjustment = levelsCompleted * -1;

            waveComplete = false;
            waveLength += waveLengthInc;

            loadingNewLevel = true;
            Application.LoadLevel("waveArena");            
        }
    }

    private void DoNewLevelSetup()
    {
        float newSpawnTimer = Mathf.Max(minimumSpawnTimer, SpawnManager.Instance.spawnTimer + spawnTimeAdjustment);
        SpawnManager.Instance.spawnTimer = newSpawnTimer;
        
        timeRemaining = waveLength;
    }

    public void OnLevelWasLoaded(int level)
    {
        loadingNewLevel = false;
        DoNewLevelSetup();
    }
}
