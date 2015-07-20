using UnityEngine;
using System.Collections;

public class BeatLevel : MonoBehaviour {

    public GameLevel levelToBeat;

    public void DoBeatLevel()
    {
        Globals.Instance.BeatLevel(levelToBeat);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
