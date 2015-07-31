using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuCameraControl : MonoBehaviour {

    public ScreenShakeAdjuster shakeAdjuster;
    public CameraToggle rotationAdjuster;
    public CameraToggle waterFilterAdjuster;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        UpdateCameraValues();
    }

    public void UpdateCameraValues()
    {
        Debug.Log("Camera Control: Updating Values");
        shakeAdjuster.shakeSlider.value = Globals.Instance.screenShakeFactor;
        shakeAdjuster.AdjustShakeText();
        rotationAdjuster.camToggle.isOn = Globals.Instance.cameraSpinEnabled;
        rotationAdjuster.AdjustOptionText();
        waterFilterAdjuster.camToggle.isOn = Globals.Instance.IsWaterFilterOn();
        waterFilterAdjuster.AdjustOptionText();
        //masterVolume.audioSlider.value = AudioManager.Instance.GetMasterVolume();
        //musicVolume.audioSlider.value = AudioManager.Instance.GetMusicVolume();
        //sfxVolume.audioSlider.value = AudioManager.Instance.GetSFXVolume();
    }
}
