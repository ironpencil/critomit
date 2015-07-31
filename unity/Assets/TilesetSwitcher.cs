using UnityEngine;
using System.Collections;

public class TilesetSwitcher : MonoBehaviour {

    public bool doSwitch = false;

    public float switchDelayMin = 5.0f;
    public float switchDelayMax = 10.0f;
    
    private float nextSwitchTime = 0.0f;

	// Use this for initialization
	void OnEnable () {
        nextSwitchTime = Time.time + Random.Range(switchDelayMin, switchDelayMax);
	
	}
	
	// Update is called once per frame
	void Update () {
        if (doSwitch && Time.time > nextSwitchTime)
        {
            TileMapBuilder.Instance.GenerateRandomTileset();
            nextSwitchTime = Time.time + Random.Range(switchDelayMin, switchDelayMax);
        }
	}
}
