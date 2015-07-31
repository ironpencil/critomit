using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventTextManager : Singleton<EventTextManager> {


    public float flashOnTime = 0.4f;
    public float flashOffTime = 0.1f;
    public int flashCount = 2;

    public Color onColor;
    public Color offColor;

    private IEnumerator currentEvent;
    
	// Use this for initialization
	public override void Start () {
        base.Start();
        
	}
	
	// Update is called once per frame
	void Update () {

        if (DebugLogger.DEBUG_MODE.Equals("DEBUG"))
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                AddEvent("Test Message", 1.0f, true);
            }
        }
	
	}
    

    public void AddEvent(string text, float duration, bool doFlash)
    {
        if (ObjectManager.Instance.eventText != null)
        {
            StartCoroutine(AddEventAndWait(text, duration, doFlash));
        }
    }


    private IEnumerator AddEventAndWait(string text, float duration, bool doFlash)
    {
        if (currentEvent != null)
        {
            StopCoroutine(currentEvent);
        }

        currentEvent = DisplayEventText(text, duration, doFlash);
        yield return StartCoroutine(currentEvent);
    }

    private IEnumerator DisplayEventText(string text, float duration, bool doFlash)
    {             
        ObjectManager.Instance.eventText.text = text;

        if (doFlash)
        {
            for (int i = 0; i < flashCount; i++)
            {
                ObjectManager.Instance.eventText.color = onColor;

                yield return new WaitForSeconds(flashOnTime);

                ObjectManager.Instance.eventText.color = offColor;

                yield return new WaitForSeconds(flashOffTime);
            }
        }

        ObjectManager.Instance.eventText.color = onColor;

        yield return new WaitForSeconds(duration);

        ObjectManager.Instance.eventText.color = offColor;

        currentEvent = null;
    }
    
}
