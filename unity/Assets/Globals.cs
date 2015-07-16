using UnityEngine;
using System.Collections;

public class Globals : Singleton<Globals> {

    public GameState currentState = GameState.Title;

    public void LoadGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Title:
                Application.LoadLevel("title");
                currentState = GameState.Title;
                break;
            case GameState.Arena:
                Application.LoadLevel("waveArena");
                currentState = GameState.Arena;
                break;
            default:
                break;
        }
    }
}
