using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour {

    public Image mask;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (mask == null)
        {
            mask = gameObject.GetComponent<Image>();
        }
	}
}
