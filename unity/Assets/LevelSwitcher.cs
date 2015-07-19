using UnityEngine;
using System.Collections;

public class LevelSwitcher : MonoBehaviour {

    public GameLevel levelToLoad;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            switch (levelToLoad)
            {
                case GameLevel.Title:                    
                    Globals.Instance.LoadGameState(GameState.Title);
                    break;
                case GameLevel.Lobby:
                    Globals.Instance.LoadGameState(GameState.Lobby);
                    break;
                case GameLevel.Arena:
                    Globals.Instance.LoadGameState(GameState.Arena);
                    break;
                default:
                    break;
            }

        }
    }
}
