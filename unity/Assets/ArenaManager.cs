using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaManager : Singleton<ArenaManager> {

    public int wavesCompleted = 0;

    public bool waveComplete = false;

    private bool loadingNewLevel = false;    

    void OnEnable()
    {
        if (Globals.Instance.currentState != GameState.Arena)
        {
            this.enabled = false;
        }

        Reset();
    }

    void Start()
    {
        base.Start();

        StartWave();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CompleteWave();
            StartWave();
        }

        if (loadingNewLevel)
        {
            Debug.LogError("!!!!!! Update called while Loading new level!!!!!!!");
        }

    }

    void LateUpdate()
    {
        //if (waveComplete && SpawnManager.Instance.EnemiesRemaining == 0)
        if (SpawnManager.Instance.EnemiesRemaining == 0 &&
            SpawnManager.Instance.SpawnersRemaining == 0)
        {
            Debug.Log("Level complete!");

            CompleteWave();
            LoadNextLevel();            
        }
    }

    private void DoNewLevelSetup()
    {
        MutatorManager.Instance.GenerateNewLevelMutators();
    }

    public void OnLevelWasLoaded(int level)
    {
        loadingNewLevel = false;
        StartWave();
    }

    public void Reset()
    {
        Debug.Log("Resetting Arena");
        wavesCompleted = 0;
        //also clear all mutators
    }

    public void StartWave()
    {
        DoNewLevelSetup();        
    }

    public void CompleteWave()
    {
        wavesCompleted++;

        SpawnManager.Instance.ClearEnemies();
        ObjectManager.Instance.player.GetComponent<PlayerDamageManager>().FullHeal();
    }

    public void LoadNextLevel()
    {
        loadingNewLevel = true;
        Application.LoadLevel("waveArena");
    }
}
