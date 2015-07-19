using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class GUIManager : Singleton<GUIManager> {

    private const int IGNORE_RAYCAST_LAYER = 2;

    public Canvas canvas;
    public ScreenFader screenFader;

    public bool isFading = false;

	// Use this for initialization
	void Start () {
        base.Start();

        if (canvas != null)
        {
            canvas.gameObject.SetActive(true);
        }        
	}
	
	// Update is called once per frame
	void Update () {
        if (screenFader == null)
        {
            screenFader = FindObjectOfType<ScreenFader>();
        }        
	}

    public static bool IsMouseInputBlocked()
    {
        bool blockMouseInput = false;

        bool mouseOverGameObject = EventSystem.current.IsPointerOverGameObject();
        
        if (mouseOverGameObject)
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

            if (selectedObject != null && selectedObject.layer != IGNORE_RAYCAST_LAYER)
            {
                blockMouseInput = true;
            }    
        }

        return blockMouseInput;
    }

    public void FadeScreen(float startOpacity, float endOpacity, float duration)
    {        
        if (screenFader && !isFading)
        {
            StartCoroutine(DoFadeScreen(startOpacity, endOpacity, duration));
        }
    }

    private IEnumerator DoFadeScreen(float startOpacity, float endOpacity, float duration)
    {
        isFading = true;
        Color maskColor = screenFader.mask.color;

        maskColor.a = startOpacity;

        screenFader.mask.color = maskColor;

        float currentDuration = 0.0f;

        while (currentDuration < duration)
        {
            yield return null;
            
            currentDuration += Time.deltaTime;

            float newOpacity = Mathf.Lerp(startOpacity, endOpacity, currentDuration / duration);

            maskColor.a = newOpacity;

            screenFader.mask.color = maskColor;
        }

        maskColor.a = endOpacity;

        screenFader.mask.color = maskColor;

        isFading = false;
    }
}
