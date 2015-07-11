using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {

    public float magnitude = 1.0f;
    public float sustainTime = 0.2f;
    public float decayTime = 0.2f;

    private CameraShake shakeCam;
	// Use this for initialization
	void Start () {
        shakeCam = Camera.main.GetComponent<CameraShake>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Shake()
    {
        shakeCam.ShakeCamera(magnitude, sustainTime, decayTime);
    }
}
