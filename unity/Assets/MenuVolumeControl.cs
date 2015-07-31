using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuVolumeControl : MonoBehaviour {

    public VolumeAdjuster masterVolume;
    public VolumeAdjuster musicVolume;
    public VolumeAdjuster sfxVolume;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        UpdateVolumeSliders();
    }

    public void UpdateVolumeSliders()
    {
        DebugLogger.Log("Volume Control: Updating Sliders");
        masterVolume.audioSlider.value = AudioManager.Instance.GetMasterVolume();
        musicVolume.audioSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxVolume.audioSlider.value = AudioManager.Instance.GetSFXVolume();
    }
}
