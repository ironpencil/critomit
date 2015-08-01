using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Globals : Singleton<Globals> {

    public GameState currentState;

    public GameLevel currentLevel;

    private bool loadingLevel = false;
    private GameLevel targetLevel;

    public HashSet<GameLevel> levelsBeaten = new HashSet<GameLevel>();

    public bool paused = false;
    public bool acceptPlayerGameInput = true;

    public bool playIntro = true;
    public IntroPanel firstPanel;

    private bool firstLevel = true;

    public const int THE_VOID_LAYER = 31;   //void layer has no collisions with anything

    public override void Start()
    {
        base.Start();

        if (this == null) { return; }

        targetLevel = currentLevel;

        if (playIntro && firstPanel != null)
        {
            //play intro   
            acceptPlayerGameInput = false;
            GUIManager.Instance.FadeScreen(1.0f, 1.0f, 0.0f);
            firstPanel.DisplayPanel();
        }
        else
        {
            StartCoroutine(DoNewLevelSetup(currentLevel));
        }
    }

    public void IntroFinished()
    {
        if (playIntro)
        {
            playIntro = false;
            //acceptPlayerGameInput = true;
            StartCoroutine(DoNewLevelSetup(currentLevel));
        }
    }
    
    public float screenShakeFactor = 1.0f;
    public float screenShakeMax = 5.0f;
    public bool cameraSpinEnabled = true;

    public int equippedPrimaryWeapon = 0;
    public int equippedSecondaryWeapon = 0;



    //private void LoadGameState(GameState state)
    //{
    //    switch (state)
    //    {
    //        case GameState.Title:
    //            currentState = GameState.Title;
    //            LoadLevel(GameLevel.Title);
    //            break;
    //        case GameState.Lobby:
    //            currentState = GameState.Lobby;
    //            LoadLevel(GameLevel.Lobby);
    //            break;
    //        case GameState.Arena:
    //            currentState = GameState.Arena;
    //            ArenaManager.Instance.enabled = true;
    //            LoadLevel(GameLevel.Arena);
    //            break;
    //        case GameState.Campaign:
    //            currentState = GameState.Campaign;
    //            ArenaManager.Instance.enabled = false;
    //            LoadLevel(GameLevel.Level1);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    public void LoadLevel(GameLevel level)
    {
        if (loadingLevel) { return; }

        loadingLevel = true;
        string levelName = "";

        switch (level)
        {
            //case GameLevel.Title:
            //    levelName = "title";
            //    currentState = GameState.Title;
            //    break;
            case GameLevel.Lobby:
                levelName = "lobby";
                if (currentState != GameState.Lobby)
                {                    
                    AudioManager.Instance.TransitionToLobby();
                }
                currentState = GameState.Lobby;
                break;
            case GameLevel.Arena:
                levelName = "waveArena";
                if (currentState != GameState.Arena)
                {                    
                    AudioManager.Instance.TransitionToArena();
                }
                currentState = GameState.Arena;
                break;
            //case GameLevel.Level1:
            //    levelName = "level1";
            //    currentState = GameState.Campaign;
            //    break;
            //case GameLevel.Level2:
            //    levelName = "level2";
            //    currentState = GameState.Campaign;
            //    break;
            //case GameLevel.Level3:
            //    levelName = "level3";
            //    currentState = GameState.Campaign;
            //    break;
            default:
                DebugLogger.LogError("Attempting to load invalid level!");
                loadingLevel = false;
                return;
        }

        
        //ArenaManager.Instance.enabled = (currentState == GameState.Arena);

        acceptPlayerGameInput = false;

        if (playerDied)
        {
            equippedPrimaryWeapon = 0;
            equippedSecondaryWeapon = 0;
        }
        else
        {
            try
            {
                equippedPrimaryWeapon = ObjectManager.Instance.weaponController.GetEquippedWeaponIndex(WeaponLocation.Primary);
                equippedSecondaryWeapon = ObjectManager.Instance.weaponController.GetEquippedWeaponIndex(WeaponLocation.Secondary);
            }
            catch
            {
                equippedPrimaryWeapon = 0;
                equippedSecondaryWeapon = 0;
            }
        }

        targetLevel = level;
        StartCoroutine(DoLoadLevel(levelName));
    }

    private IEnumerator DoLoadLevel(string levelName)
    {
        if (ObjectManager.Instance.followCam != null)
        {
            ObjectManager.Instance.followCam.followTarget = null;
        }

        yield return StartCoroutine(GUIManager.Instance.DoAutoMessageBoxes(false));

        //yield return new WaitForSeconds(2.0f);

        GUIManager.Instance.FadeScreen(0.0f, 1.0f, 0.5f);

        while (GUIManager.Instance.isFading)
        {
            yield return null;
        }

        //FOR SOME REASON WE NEED TO WAIT HERE OR THE LOADLEVEL WILL CAUSE UNITY TO CRASH
        //SO DON'T FUCKIN TOUCH THIS vvvvvvvvvv
        yield return new WaitForSeconds(0.5f);
        //SERIOUSLY DON'T ^^^^^^^^^^

        Application.LoadLevel(levelName);

    }

    private void DoFadeScreenIn()
    {
        GUIManager.Instance.FadeScreen(1.0f, 0.0f, 0.5f);
    }

    private void OnLevelWasLoaded(int level)
    {
        currentLevel = targetLevel;
        StartCoroutine(DoNewLevelSetup(currentLevel));
    }

    private IEnumerator DoNewLevelSetup(GameLevel level)
    {
        loadingLevel = false;
        playerDied = false;

        //yield return new WaitForSeconds(0.1f);

        
        //if (level == GameLevel.Arena)
        //{
        //    Pause(true);
        //}        

        DebugLogger.Log("Globals: New Level Setup");

        //try
        //{
        //    ObjectManager.Instance.weaponController.primaryWeaponIndex = equippedPrimaryWeapon;
        //    ObjectManager.Instance.weaponController.secondaryWeaponIndex = equippedSecondaryWeapon;
        //}
        //catch { }

        DoFadeScreenIn();

        yield return null;

        

        EnableWaterFilter(waterFilterEnabled);

        float taintChance = Mathf.Min(ArenaManager.Instance.wavesCompleted * 0.1f, 0.5f);        

        TileMapBuilder.Instance.taintChance = taintChance;

        TileMapBuilder.Instance.randomizedTainted = true;

        acceptPlayerGameInput = true;

        if (level == GameLevel.Arena)
        {            
            acceptPlayerGameInput = false;
            
            TileMapBuilder.Instance.LoadRandomMap(); //use a random available map with a random tileset
            //AudioManager.Instance.TransitionToArena();
            ArenaManager.Instance.PrepareNextWave();
        }
        else
        {
            TileMapBuilder.Instance.LoadPreferredMap(); //use the lobby map

            if (!firstLevel)
            {
                TileMapBuilder.Instance.GenerateRandomTileset(); //apply a random tileset
            }            

            AudioManager.Instance.UnduckMusic();
        }

        firstLevel = false;

        while (GUIManager.Instance.isFading)
        {
            yield return null;
        }        

        //yield return new WaitForSeconds(0.1f);
                
        yield return StartCoroutine(GUIManager.Instance.DoAutoMessageBoxes(true));

        //if (level == GameLevel.Arena)
        //{
        //    Pause(true);
        //}
    }

    public bool HasBeatenLevel(GameLevel level)
    {
        return levelsBeaten.Contains(level);
    }

    public bool BeatLevel(GameLevel level)
    {
        //returns true if this level is being added, false if it already existed
        return levelsBeaten.Add(level);
    }

    private bool playerDied = false;

    public void PlayerDied()
    {
        DebugLogger.Log("Player Died. CurrentLevel = " + Enum.GetName(typeof(GameLevel), currentLevel));

        playerDied = true;        

        switch (currentLevel)
        {
            //case GameLevel.Title:
            //    ObjectManager.Instance.DestroyPlayer();
            //    StartCoroutine(WaitAndLoadLevel(5.0f, GameLevel.Lobby));
            //    break;
            case GameLevel.Lobby:
                ObjectManager.Instance.DestroyPlayer();
                StartCoroutine(WaitAndLoadLevel(5.0f, GameLevel.Lobby));
                break;
            case GameLevel.Arena:                
                ObjectManager.Instance.DestroyPlayer();
                ArenaManager.Instance.EndWave();
                //ArenaManager.Instance.Reset();
                StartCoroutine(WaitAndLoadLevel(5.0f, GameLevel.Lobby));
                break;
            //case GameLevel.Level1:
            //    ObjectManager.Instance.DestroyAndRespawnPlayer();
            //    break;
            //case GameLevel.Level2:
            //    ObjectManager.Instance.DestroyAndRespawnPlayer();
            //    break;
            //case GameLevel.Level3:
            //    ObjectManager.Instance.DestroyAndRespawnPlayer();
            //    break;
            default:
                break;
        }
    }

    public IEnumerator WaitAndLoadLevel(float waitTime, GameLevel level)
    {
        yield return new WaitForSeconds(waitTime);

        LoadLevel(level);
    }

    public void Pause(bool pause)
    {
        if (isQuitting) { return; }

        paused = pause;

        if (paused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    [ContextMenu("Toggle Pause")]
    public void TogglePause()
    {
        Pause(!paused);
    }


    public void ToggleMenu()
    {
        try
        {
            MenuDialog menu = ObjectManager.Instance.menuDialog;

            if (menu != null)
            {
                ButtonSelectionHandler menuButton = menu.menuButton;

                if (menuButton.button.IsInteractable() && !loadingLevel)
                {
                    menu.ToggleDisplayed();
                }
            }
        }
        catch { }
    }

    private bool waterFilterEnabled = true;

    public void EnableWaterFilter(bool enabled)
    {
        GameObject waterFilter = ObjectManager.Instance.waterFilter;

        if (waterFilter != null)
        {
            waterFilter.SetActive(enabled);
        }

        waterFilterEnabled = enabled;
    }

    public bool IsWaterFilterOn()
    {
        return waterFilterEnabled;
    }

    public bool isQuitting = false;

    public void DoQuit()
    {
        Pause(false);
        isQuitting = true;        
        StartCoroutine(WaitAndQuit(1.0f));
    }

    private IEnumerator WaitAndQuit(float time)
    {
        yield return new WaitForSeconds(time);

        Application.Quit();
    }
}
