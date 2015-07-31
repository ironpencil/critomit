using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ScreenShakeAdjuster : MonoBehaviour, IBeginDragHandler, IEndDragHandler {


    public Slider shakeSlider;
    public Text shakeText;

    public bool playSoundWhileSliding = false;
    public SoundEffectHandler slidingSound;

    public float playSoundDelay = 1.0f;
    private float lastPlayedTime = 0.0f;
    private bool isDragging = false;

	// Use this for initialization
	void Start () {
        if (shakeSlider == null)
        {
            shakeSlider = gameObject.GetComponent<Slider>();
        }
	
	}

    void Update()
    {
        if (isDragging)
        {
            PlaySlidingSound();
        }
    }

    public void AdjustShake()
    {
        Globals.Instance.screenShakeFactor = shakeSlider.value;

        AdjustShakeText();
        //PlaySlidingSound();
    }

    public void AdjustShakeText()
    {
        if (shakeSlider.value > 0)
        {
            if (shakeSlider.value == shakeSlider.maxValue)
            {
                shakeText.text = "VLAMBEER IT";
            }
            else
            {
                shakeText.text = "SHAKE IT " + shakeSlider.value + "X";
            }
        }
        else
        {
            shakeText.text = "SHAKE IT OFF";
        }
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
