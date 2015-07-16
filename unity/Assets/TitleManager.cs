using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

    public void PlayGame()
    {
        Globals.Instance.LoadGameState(GameState.Arena);
    }
}
