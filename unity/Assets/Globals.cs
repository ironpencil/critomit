using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Globals : Singleton<Globals> {

    public GameState currentState;
    public GameLevel currentLevel;

    private bool loadingLevel = false;
    private GameLevel targetLevel;

    public HashSet<GameLevel> levelsBeaten = new HashSet<GameLevel>();

    public override void Start()
    {
        base.Start();

        StartCoroutine(DoNewLevelSetup(0));
    }

    public void LoadGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Title:
                currentState = GameState.Title;
                LoadLevel(GameLevel.Title);
                break;
            case GameState.Lobby:
                currentState = GameState.Lobby;
                LoadLevel(GameLevel.Lobby);
                break;
            case GameState.Arena:
                currentState = GameState.Arena;
                ArenaManager.Instance.enabled = true;
                LoadLevel(GameLevel.Arena);
                break;
            case GameState.Campaign:
                currentState = GameState.Campaign;
                ArenaManager.Instance.enabled = false;
                LoadLevel(GameLevel.Level1);
                break;
            default:
                break;
        }
    }

    public void LoadLevel(GameLevel level)
    {
        if (loadingLevel) { return; }

        loadingLevel = true;
        string levelName = "";

        switch (level)
        {
            case GameLevel.Title:
                levelName = "title";
                currentState = GameState.Title;
                break;
            case GameLevel.Lobby:
                levelName = "lobby";
                currentState = GameState.Lobby;
                break;
            case GameLevel.Arena:
                levelName = "waveArena";
                currentState = GameState.Arena;
                break;
            case GameLevel.Level1:
                levelName = "level1";
                currentState = GameState.Campaign;
                break;
            case GameLevel.Level2:
                levelName = "level2";
                currentState = GameState.Campaign;
                break;
            case GameLevel.Level3:
                levelName = "level3";
                currentState = GameState.Campaign;
                break;
            default:
                Debug.LogError("Attempting to load invalid level!");
                loadingLevel = false;
                return;
        }
        
        //ArenaManager.Instance.enabled = (currentState == GameState.Arena);

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
        StartCoroutine(DoNewLevelSetup(level));
    }

    private IEnumerator DoNewLevelSetup(int level)
    {
        loadingLevel = false;
        currentLevel = targetLevel;

        //yield return new WaitForSeconds(0.1f);

        DoFadeScreenIn();

        while (GUIManager.Instance.isFading)
        {
            yield return null;
        }        

        //yield return new WaitForSeconds(0.1f);
                
        yield return StartCoroutine(GUIManager.Instance.DoAutoMessageBoxes(true));
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

    public void PlayerDied()
    {
        switch (currentLevel)
        {
            case GameLevel.Title:
                ObjectManager.Instance.DestroyPlayer();
                StartCoroutine(WaitAndLoadLevel(2.0f, GameLevel.Lobby));
                break;
            case GameLevel.Lobby:
                ObjectManager.Instance.DestroyPlayer();
                StartCoroutine(WaitAndLoadLevel(2.0f, GameLevel.Lobby));
                break;
            case GameLevel.Arena:                
                ObjectManager.Instance.DestroyPlayer();
                ArenaManager.Instance.EndWave();
                StartCoroutine(WaitAndLoadLevel(2.0f, GameLevel.Lobby));
                break;
            case GameLevel.Level1:
                ObjectManager.Instance.DestroyAndRespawnPlayer();
                break;
            case GameLevel.Level2:
                ObjectManager.Instance.DestroyAndRespawnPlayer();
                break;
            case GameLevel.Level3:
                ObjectManager.Instance.DestroyAndRespawnPlayer();
                break;
            default:
                break;
        }
    }

    public IEnumerator WaitAndLoadLevel(float waitTime, GameLevel level)
    {
        yield return new WaitForSeconds(waitTime);

        LoadLevel(level);
    }
}
