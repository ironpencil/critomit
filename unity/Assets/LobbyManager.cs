using UnityEngine;
using System.Collections;

public class LobbyManager : MonoBehaviour {

    public LevelSwitcher level2;
    public LevelSwitcher level3;

	// Use this for initialization
	void Start () {
        level2.gameObject.SetActive(Globals.Instance.HasBeatenLevel(GameLevel.Level1));
        level3.gameObject.SetActive(Globals.Instance.HasBeatenLevel(GameLevel.Level2));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
