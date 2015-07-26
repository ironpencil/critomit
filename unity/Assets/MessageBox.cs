using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageBox : MonoBehaviour {

    public RectTransform rect;
    public CanvasGroup canvasGroup;

    public Vector2 closedSize = new Vector2(76.0f, 76.0f);
    public Vector2 openSize = new Vector2(400.0f, 300.0f);

    public float openTime = 1.0f;
    public float closeTime = 1.0f;

    public bool isOpen = false;
    public bool isResizing = false;

    public bool visibleWhileClosed = false;
    public float visibleAlpha = 1.0f;
    public float invisibleAlpha = 0.0f;

    public List<Transform> disableWhileClosed = new List<Transform>();

    public enum ResizeStyle
    {
        Simultaneous,
        WidthFirst,
        HeightFirst
    }

    public ResizeStyle openStyle = ResizeStyle.WidthFirst;
    public ResizeStyle closeStyle = ResizeStyle.HeightFirst;
    
	// Use this for initialization
	void Start () {
        if (rect == null)
        {
            rect = gameObject.GetComponent<RectTransform>();
        }

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
        }

        if (isOpen)
        {
            SetOpen();
        }
        else
        {
            SetClosed();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator ToggleAndWait()
    {
        Debug.Log("Toggling - currently " + isOpen);

        if (isOpen)
        {
            yield return StartCoroutine(CloseAndWait());
        }
        else
        {
            yield return StartCoroutine(OpenAndWait());
        }
    }

    public void ToggleWithoutWait()
    {
        StartCoroutine(ToggleAndWait());
    }
    
    public IEnumerator OpenAndWait()
    {
        SetVisible(true);
        Debug.Log("Opening");
        yield return StartCoroutine(Resize(closedSize, openSize, openTime, openStyle));
        isOpen = true;
        EnableChildren(true);
    }
    
    public IEnumerator CloseAndWait()
    {
        Debug.Log("Closing");
        isOpen = false; 
        EnableChildren(false);

        yield return StartCoroutine(Resize(openSize, closedSize, closeTime, closeStyle));
        
        if (!visibleWhileClosed)
        {
            SetVisible(false);
        }
    }

    public void SetVisible(bool visible)
    {
        if (visible)
        {
            canvasGroup.alpha = visibleAlpha;
        }
        else
        {
            canvasGroup.alpha = invisibleAlpha;
        }
    }

    public void EnableChildren(bool enable)
    {
        foreach (Transform child in disableWhileClosed)
        {
            child.gameObject.SetActive(enable);
        }
    }

    public void SetOpen()
    {
        float originalOpenTime = openTime;
        openTime = 0.0f;
        StartOpen();
        openTime = originalOpenTime;
    }

    public void SetClosed()
    {
        float originalCloseTime = closeTime;
        closeTime = 0.0f;
        StartClose();
        closeTime = originalCloseTime;
    }

    [ContextMenu("Open")]
    public void StartOpen()
    {
        StartCoroutine(OpenAndWait());
    }

    [ContextMenu("Close")]
    public void StartClose()
    {
        StartCoroutine(CloseAndWait());        
    }          

    private IEnumerator Resize(Vector2 startSize, Vector2 targetSize, float duration, ResizeStyle resizeStyle)
    {
        isResizing = true;

        switch (resizeStyle)
        {
            case ResizeStyle.Simultaneous:
                StartCoroutine(ResizeAxis(RectTransform.Axis.Horizontal, startSize.x, targetSize.x, duration));
                yield return StartCoroutine(ResizeAxis(RectTransform.Axis.Vertical, startSize.y, targetSize.y, duration));
                break;
            case ResizeStyle.WidthFirst:
                yield return StartCoroutine(ResizeAxis(RectTransform.Axis.Horizontal, startSize.x, targetSize.x, duration * 0.5f));
                yield return StartCoroutine(ResizeAxis(RectTransform.Axis.Vertical, startSize.y, targetSize.y, duration * 0.5f));
                break;
            case ResizeStyle.HeightFirst:
                yield return StartCoroutine(ResizeAxis(RectTransform.Axis.Vertical, startSize.y, targetSize.y, duration * 0.5f));
                yield return StartCoroutine(ResizeAxis(RectTransform.Axis.Horizontal, startSize.x, targetSize.x, duration * 0.5f));
                break;
            default:
                break;
        }

        isResizing = false;
        //float elapsedTime = 0.0f;

        //while (elapsedTime < duration)
        //{
        //    float percentComplete = elapsedTime / duration;
        //    float newWidth = Mathf.Lerp(startSize.x, targetSize.x, percentComplete);
        //    float newHeight = Mathf.Lerp(startSize.y, targetSize.y, percentComplete);
            
        //    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        //    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

        //    yield return null;

        //    elapsedTime += Time.deltaTime;
        //}

        //rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetSize.x);
        //rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetSize.y);
    }

    private IEnumerator ResizeAxis(RectTransform.Axis axis, float startSize, float targetSize, float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float percentComplete = elapsedTime / duration;
            float newSize = Mathf.Lerp(startSize, targetSize, percentComplete);            

            rect.SetSizeWithCurrentAnchors(axis, newSize);

            float currentTime = Time.realtimeSinceStartup;

            yield return null;

            float realDeltaTime = Time.realtimeSinceStartup - currentTime;

            elapsedTime += realDeltaTime;
        }

        rect.SetSizeWithCurrentAnchors(axis, targetSize);
    }
}
