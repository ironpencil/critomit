using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaManager : Singleton<ArenaManager> {

    public int wavesCompleted = 0;

    public bool waveComplete = false;

    [SerializeField]
    private bool waveActive = false;

    public SoundEffectHandler waveCompleteSound;

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
            if (DebugLogger.DEBUG_MODE.Equals("DEBUG"))
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    CompleteWave(false);
                    StartWave();
                }

                if (Input.GetKeyDown(KeyCode.L))
                {
                    CompleteWave(true);
                    //LoadNextLevel();
                }
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
                DebugLogger.Log("Level complete!");

                CompleteWave(true);
                //LoadNextLevel();
            }
        }
    }

    //public override void OnLevelWasLoaded(int level)
    //{
    //    base.OnLevelWasLoaded(level);

    //    //if (this == null) { return; }

    //    //DebugLogger.Log("ArenaManager[" + gameObject.GetInstanceID() + "]::OnLevelWasLoaded(" + level + ")"); 

    //    //if (Globals.Instance.currentState == GameState.Arena)
    //    //{
    //    //    StartWave();
    //    //}
    //}

    public void Reset()
    {
        DebugLogger.Log("Resetting Arena");
        wavesCompleted = 0;
        MutatorManager.Instance.Reset();
        //also clear all mutators
    }

    public void PrepareNextWave()
    {
        ScoreManager.Instance.VerifyMinimumMultiplier();
        ScoreManager.Instance.ResetKillCounter();
        ScoreManager.Instance.ResetHighestMultiplier();
        ScoreManager.Instance.RefreshPointsDisplay();        
        ObjectManager.Instance.waveText.text = (wavesCompleted + 1).ToString();
        MutatorManager.Instance.GenerateNewLevelMutators();

        ObjectManager.Instance.startWaveDialog.PrepareDialog();
    }

    

    public void StartWave()
    {        
        DebugLogger.Log("StartWave()");
        waveActive = true;
        MutatorManager.Instance.ApplyPendingMutators();
        //ScoreManager.Instance.RefreshPointsDisplay();
        //ObjectManager.Instance.waveText.text = (wavesCompleted + 1).ToString();
        SpawnManager.Instance.StartSpawners();
    }

    public void CompleteWave(bool showDialog)
    {        

        EndWave();

        wavesCompleted++;

        //make sure player doesn't die
        PlayerDamageManager pdm = ObjectManager.Instance.player.GetComponent<PlayerDamageManager>();

        if (pdm != null)
        {
            if (pdm.MarkedForDeath)
            {
                return; // they're already dead, can't do anything
            }

            pdm.Invulnerable = true;
        }

        EventTextManager.Instance.AddEvent(@"!! (GOT EM!> \(^.^')/ !!", 5.0f, true);

        //only play the sound if we're not showing the dialog
        if (waveCompleteSound != null && !showDialog)
        {
            waveCompleteSound.PlayEffect();
        }

        SpawnManager.Instance.StopSpawners();
        SpawnManager.Instance.ClearEnemies();
        //ObjectManager.Instance.player.GetComponent<PlayerDamageManager>().FullHeal();

        if (showDialog)
        {
            ObjectManager.Instance.endWaveDialog.PrepareDialog();
            ObjectManager.Instance.endWaveDialog.ApplyTotalWaveBonus();
            ObjectManager.Instance.endWaveDialog.endWaveMessageBox.StartOpen();
            //Globals.Instance.acceptPlayerGameInput = false;
        }
    }

    public void EndWave()
    {
        DebugLogger.Log("EndWave()");
        waveActive = false;
    }

    public void LoadNextLevel()
    {
        //Application.LoadLevel("waveArena");
        //this is called from the End Wave dialog
        StartCoroutine(Globals.Instance.WaitAndLoadLevel(1.0f, GameLevel.Arena));
    }
}
