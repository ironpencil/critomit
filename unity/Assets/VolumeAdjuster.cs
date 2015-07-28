using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VolumeAdjuster : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

    public enum AudioGroup
    {
        Master,
        Music,
        SFX
    }

    public AudioGroup audioToAdjust;
    public Slider audioSlider;

    public bool playSoundWhileSliding = false;
    public SoundEffectHandler slidingSound;

    public float playSoundDelay = 1.0f;
    private float lastPlayedTime = 0.0f;
    private bool isDragging = false;

	// Use this for initialization
	void Start () {
        if (audioSlider == null)
        {
            audioSlider = gameObject.GetComponent<Slider>();
        }
	
	}

    void Update()
    {
        if (isDragging)
        {
            PlaySlidingSound();
        }
    }

    public void AdjustVolume()
    {
        switch (audioToAdjust)
        {
            case AudioGroup.Master:
                AudioManager.Instance.SetMasterVolume(audioSlider.value);
                break;
            case AudioGroup.Music:
                AudioManager.Instance.SetMusicVolume(audioSlider.value);
                break;
            case AudioGroup.SFX:
                AudioManager.Instance.SetSFXVolume(audioSlider.value);
                break;
            default:
                break;
        }

        //PlaySlidingSound();
    }

    public void OnBeginDrag(PointerEventData data)
    {
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData data)
    {
        isDragging = false;
    }

    public void PlaySlidingSound()
    {
        if (playSoundWhileSliding && slidingSound != null && Time.realtimeSinceStartup > lastPlayedTime + playSoundDelay)
        {
            slidingSound.PlayEffect();
            lastPlayedTime = Time.realtimeSinceStartup;
        }

    }


}
