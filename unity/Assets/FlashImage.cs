using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashImage : MonoBehaviour {

    public Image imageToFlash;

    public Color flashOnColor = Color.red;
    public Color flashOffColor = Color.red;

    public float flashOnDuration = 0.5f;
    public float flashOffDuration = 0.25f;

    public bool ignoreTimescale = true;

    public bool doFlash = true;

    private bool flashOn = false;

    private float currentTime = 0.0f;

    private float flashElapsedTime = 0.0f;
    private float flashDuration = 0.0f;

    // Use this for initialization
	void Start () {
	    currentTime = Time.realtimeSinceStartup;
        if (imageToFlash == null)
        {
            imageToFlash = gameObject.GetComponent<Image>();
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (!doFlash) {
            return;
        }

        float deltaTime;

        if (ignoreTimescale)
        {
            deltaTime = Time.realtimeSinceStartup - currentTime;
            currentTime = Time.realtimeSinceStartup;
        }
        else
        {
            deltaTime = Time.deltaTime;
        }

        flashElapsedTime += deltaTime;

        if (flashElapsedTime > flashDuration)
        {
            Flash();
        }
	}

    public void Flash()
    {
        flashOn = !flashOn;

        if (flashOn)
        {
            imageToFlash.color = flashOnColor;
            flashDuration = flashOnDuration;
        }
        else
        {
            imageToFlash.color = flashOffColor;
            flashDuration = flashOffDuration;
        }

        flashElapsedTime = 0.0f;
    }
}
