using UnityEngine;
using System.Collections;

public class PersistOnLoad : MonoBehaviour {

    public bool doPersist = true;

	// Use this for initialization
	void Start () {
        if (doPersist)
        {
            DontDestroyOnLoad(gameObject);            
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
