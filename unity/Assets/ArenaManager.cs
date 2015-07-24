﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaManager : Singleton<ArenaManager> {

    public int wavesCompleted = 0;

    public float startingWaveLength = 10.0f;
    public float currentWaveLength = 10.0f;
    public float waveLengthInc = 5.0f;
    public float spawnTimeInc = -0.5f;

    public float timeRemaining = 0.0f;

    public bool waveComplete = false;

    private float spawnTimeAdjustment = 0.0f;
    private bool loadingNewLevel = false;

    private float minimumSpawnTimer = 1.0f;    

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
        timeRemaining = currentWaveLength;
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
            //SpawnManager.Instance.spawnEnemies = false;
        }
    }

    void LateUpdate()
    {
        //if (waveComplete && SpawnManager.Instance.EnemiesRemaining == 0)
        if (SpawnManager.Instance.EnemiesRemaining == 0 &&
            SpawnManager.Instance.SpawnersRemaining == 0)
        {
            Debug.Log("Level complete!");

            wavesCompleted++;

            spawnTimeAdjustment = wavesCompleted * spawnTimeInc;

            waveComplete = false;
            currentWaveLength += waveLengthInc;

            loadingNewLevel = true;
            Application.LoadLevel("waveArena");
        }
    }

    private void DoNewLevelSetup()
    {
        float newSpawnTimer = Mathf.Max(minimumSpawnTimer, SpawnManager.Instance.spawnTimer + spawnTimeAdjustment);
        SpawnManager.Instance.spawnTimer = newSpawnTimer;
        
        timeRemaining = currentWaveLength;
    }

    public void OnLevelWasLoaded(int level)
    {
        loadingNewLevel = false;
        DoNewLevelSetup();
    }

    public void Reset()
    {
        Debug.Log("Resetting Arena");
        wavesCompleted = 0;
        spawnTimeAdjustment = 0.0f;
        waveComplete = false;
        currentWaveLength = startingWaveLength;
    }
}