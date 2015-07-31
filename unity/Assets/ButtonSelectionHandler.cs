using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ButtonSelectionHandler : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler {

    public Button button;
    public SoundEffectHandler onSelectSound;
    public SoundEffectHandler onClickSound;

    public SoundEffectHandler confirmSound;
    public SoundEffectHandler cancelSound;

    private bool playSoundOnSelect = true;

    void Start()
    {
        if (button == null)
        {
            button = gameObject.GetComponent<Button>();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        DebugLogger.Log(gameObject.name + " selected!");

        if (playSoundOnSelect)
        {
            PlaySelectSound();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {        
        playSoundOnSelect = false;
        PlaySelectSound();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        playSoundOnSelect = true;
        //PlaySelectSound();
    }    

    public void PlaySelectSound()
    {
        if (button.IsInteractable() && onSelectSound != null)
        {
            onSelectSound.PlayEffect();
        }
    }

    public void PlayClickSound()
    {
        if (button.IsInteractable() && onClickSound != null)
        {
            onClickSound.PlayEffect();
        }
    }
}
