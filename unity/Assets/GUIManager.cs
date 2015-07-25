using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : Singleton<GUIManager> {

    private const int IGNORE_RAYCAST_LAYER = 2;

    //public Canvas canvas;
    public ScreenFader screenFader;

    public bool isFading = false;

	// Use this for initialization
    public override void Start()
    {
        base.Start();

        //if (canvas != null)
        //{
        //    canvas.gameObject.SetActive(true);
        //}        
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

    public IEnumerator DoAutoMessageBoxes(bool open)
    {
        //start to open/close all the auto messageboxes
        //then wait for all of them to finish before completing
        if (ObjectManager.Instance.autoMessageBoxes.Count != 0)
        {
            foreach (MessageBox messageBox in ObjectManager.Instance.autoMessageBoxes)
            {
                if (open)
                {
                    messageBox.StartOpen();
                }
                else
                {
                    messageBox.StartClose();
                }
            }

            bool finished = false;

            while (!finished)
            {
                finished = true;
                foreach (MessageBox messageBox in ObjectManager.Instance.autoMessageBoxes)
                {
                    if (messageBox.isResizing)
                    {
                        finished = false;
                    }
                }

                yield return 0.1f;
            }
        }
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
