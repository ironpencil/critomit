using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CustomInputText : MonoBehaviour {

    public string buttonName = "Fire1";
    public string customText = "~!~ TO SHOOT";

    public Text textToSet;

    public Dictionary<string, string> buttonAlias = new Dictionary<string, string>()
        {
            {"mouse 0", "L CLICK"},
            {"mouse 1", "R CLICK"},
            {"mouse 2", "M CLICK"}
        };

	// Use this for initialization
	void Start () {
        if (textToSet == null)
        {
            textToSet = gameObject.GetComponent<Text>();
        }

        if (textToSet != null)
        {
            

        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
