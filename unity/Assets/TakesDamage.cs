using UnityEngine;
using System.Collections;

public class TakesDamage : MonoBehaviour {

    public bool markedForDeath = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (markedForDeath)
        {
            DestroyImmediate(gameObject);
        }
	
	}
}
