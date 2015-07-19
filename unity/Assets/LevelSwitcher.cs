using UnityEngine;
using System.Collections;

public class LevelSwitcher : MonoBehaviour {

    public GameLevel levelToLoad;

    public bool activated = true;

    private bool doReactivate = false;
    private float reactivateDelay = 1.0f;
    private float reactivateTime = 0.0f;

    private bool doLoadLevel = false;

	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        if (!activated && doReactivate && Time.time > reactivateTime)
        {
            activated = true;
            doReactivate = false;
        }

        if (doLoadLevel)
        {
            activated = false;
            reactivateTime = Time.time + reactivateDelay;
            doReactivate = true;
            doLoadLevel = false;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                doLoadLevel = true;
            }
        }
    }
}
