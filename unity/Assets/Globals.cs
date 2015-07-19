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
                Application.LoadLevel("title");
                break;
            case GameState.Lobby:                
                currentState = GameState.Lobby;
                Application.LoadLevel("lobby");
                break;
            case GameState.Arena:                
                currentState = GameState.Arena;
                ArenaManager.Instance.enabled = true;
                Application.LoadLevel("waveArena");
                break;
            default:
                break;
        }
    }
}
