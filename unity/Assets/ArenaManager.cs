using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaManager : Singleton<ArenaManager> {

    public int wavesCompleted = 0;

    public bool waveComplete = false;

    [SerializeField]
    private bool waveActive = false;

    void OnEnable()
    {
        //if (Globals.Instance.currentState != GameState.Arena)
        //{
        //    this.enabled = false;
        //}

        //Reset();
    }

    void Awake()
    {
        Reset();
    }

    public override void Start()
    {
        base.Start();

        if (this == null) { return; }

        //if (Globals.Instance.currentState == GameState.Arena)
        //{
        //    StartWave();
        //}
    }

    void Update()
    {
        if (waveActive && Globals.Instance.currentState == GameState.Arena)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CompleteWave();
                StartWave();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                CompleteWave();
                LoadNextLevel();
            }

        }

    }

    void LateUpdate()
    {        
        if (waveActive && Globals.Instance.currentState == GameState.Arena)
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
    }

    //public override void OnLevelWasLoaded(int level)
    //{
    //    base.OnLevelWasLoaded(level);

    //    //if (this == null) { return; }

    //    //Debug.Log("ArenaManager[" + gameObject.GetInstanceID() + "]::OnLevelWasLoaded(" + level + ")"); 

    //    //if (Globals.Instance.currentState == GameState.Arena)
    //    //{
    //    //    StartWave();
    //    //}
    //}

    public void Reset()
    {
        Debug.Log("Resetting Arena");
        wavesCompleted = 0;
        MutatorManager.Instance.Reset();
        //also clear all mutators
    }

    public void PrepareNextWave()
    {
        ScoreManager.Instance.VerifyMinimumMultiplier();
        ScoreManager.Instance.RefreshPointsDisplay();        
        ObjectManager.Instance.waveText.text = (wavesCompleted + 1).ToString();
        MutatorManager.Instance.GenerateNewLevelMutators();

        ObjectManager.Instance.startWaveDialog.PrepareDialog();
    }

    

    public void StartWave()
    {        
        Debug.Log("StartWave()");
        waveActive = true;
        MutatorManager.Instance.ApplyPendingMutators();
        //ScoreManager.Instance.RefreshPointsDisplay();
        //ObjectManager.Instance.waveText.text = (wavesCompleted + 1).ToString();
        SpawnManager.Instance.StartSpawners();
    }

    public void CompleteWave()
    {
        EndWave();

        wavesCompleted++;

        EventTextManager.Instance.AddEvent(@"!! (GOT EM!> \(^.^')/ !!", 5.0f, true);
        
        SpawnManager.Instance.ClearEnemies();
        ObjectManager.Instance.player.GetComponent<PlayerDamageManager>().FullHeal();
    }

    public void EndWave()
    {
        Debug.Log("EndWave()");
        waveActive = false;
    }

    public void LoadNextLevel()
    {
        //Application.LoadLevel("waveArena");
        Globals.Instance.LoadLevel(GameLevel.Arena);
    }
}
