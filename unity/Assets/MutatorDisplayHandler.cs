using UnityEngine;
using System.Collections;

public class MutatorDisplayHandler : MonoBehaviour {

    public SoundEffectHandler anomaliesDetectedSound;

    public StartWaveDialog startWaveDialog;

    public bool playAlarm = false;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnEnable()
    {
        if (playAlarm && anomaliesDetectedSound != null)
        {
            anomaliesDetectedSound.PlayEffect();
            playAlarm = false;
        }
    }
}
