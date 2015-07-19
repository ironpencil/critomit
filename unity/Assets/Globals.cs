using UnityEngine;
using System.Collections;

public class Globals : Singleton<Globals> {

    public GameState currentState = GameState.Title;

    public void LoadGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Title:
                currentState = GameState.Title;
                LoadLevel("title");
                break;
            case GameState.Lobby:
                currentState = GameState.Lobby;
                LoadLevel("lobby");
                break;
            case GameState.Arena:
                currentState = GameState.Arena;
                ArenaManager.Instance.enabled = true;
                LoadLevel("waveArena");
                break;
            default:
                break;
        }
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(DoLoadLevel(levelName));
    }

    private IEnumerator DoLoadLevel(string levelName)
    {
        if (ObjectManager.Instance.FollowCam != null)
        {
            ObjectManager.Instance.FollowCam.followTarget = null;
        }

        GUIManager.Instance.FadeScreen(0.0f, 1.0f, 0.5f);

        while (GUIManager.Instance.isFading)
        {
            yield return null;
        }

        Application.LoadLevel(levelName);

    }
}
