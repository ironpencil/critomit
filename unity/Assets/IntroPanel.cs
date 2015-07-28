using UnityEngine;
using System.Collections;

public class IntroPanel : MonoBehaviour {

    public float fadeInTime = 0.25f;
    public float displayTime = 0.5f;
    public float fadeOutTime = 0.25f;

    public CanvasGroup fader;

    public IntroPanel nextPanel;
    public GameObject introParent;

    public bool lastPanel = false;

    private bool doneDisplaying = false;

    public void DisplayPanel()
    {
        if (Globals.Instance.playIntro)
        {
            StartCoroutine(DoDisplayPanel());
        }
        else
        {
            lastPanel = true;
            DoneDisplaying();
        }
    }

    public void DoneDisplaying()
    {
        if (!doneDisplaying)
        {
            doneDisplaying = true;

            this.gameObject.SetActive(false);

            if (lastPanel)
            {
                //let globals know to start
                Globals.Instance.IntroFinished();
                introParent.SetActive(false);
            }
            else
            {
                nextPanel.DisplayPanel();
            }
        }
    }

    public IEnumerator DoDisplayPanel()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeInTime)
        {
            float currentTime = Time.realtimeSinceStartup;

            yield return null;

            float deltaTime = Time.realtimeSinceStartup - currentTime;

            elapsedTime += deltaTime;

            float percentageComplete = elapsedTime / fadeInTime;

            fader.alpha = percentageComplete;
        }

        elapsedTime = 0.0f;

        while (elapsedTime < displayTime)
        {
            float currentTime = Time.realtimeSinceStartup;

            yield return null;

            float deltaTime = Time.realtimeSinceStartup - currentTime;

            elapsedTime += deltaTime;
        }

        elapsedTime = 0.0f;

        while (elapsedTime < fadeOutTime)
        {
            float currentTime = Time.realtimeSinceStartup;

            yield return null;

            float deltaTime = Time.realtimeSinceStartup - currentTime;

            elapsedTime += deltaTime;

            float percentageComplete = elapsedTime / fadeInTime;

            fader.alpha = 1 - percentageComplete;
        }

        DoneDisplaying();
    }

    
}
