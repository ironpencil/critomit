using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {

    public bool paused = false;
    public bool slowMo = false;

    public float unpausedTimescale = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TogglePause()
    {
        paused = !paused;

        if (paused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = unpausedTimescale;
        }
    }

    public void ToggleSlowmo()
    {
        slowMo = !slowMo;

        if (slowMo)
        {
            unpausedTimescale = 0.5f;
        }
        else
        {
            unpausedTimescale = 1.0f;
        }

        if (!paused)
        {
            Time.timeScale = unpausedTimescale;
        }
    }

}
