using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBarManager : MonoBehaviour {

    public List<Image> dots = new List<Image>();

    public Sprite green;
    public Sprite yellow;
    public Sprite red;
    public Sprite white;

    public int maxHealth = 100;

    public int greenThreshold = 8;
    public int yellowThreshold = 4;
    public int redThreshold = 0;

    private int numDots = 0;

    [SerializeField]
    private int currentHealth;    

    public int CurrentHealth
    {
        get { return currentHealth; }
        private set
        {
            currentHealth = value;
            RefreshDisplay();
        }
            
    }

    private void RefreshDisplay()
    {
        int totalDots = dots.Count;

        int dotsToDisplay;

        if (currentHealth == 0)
        {
            // disable all dots
            dotsToDisplay = 0;
        }
        else if (currentHealth == maxHealth)
        {
            // enable all dots
            dotsToDisplay = totalDots;
        }
        else
        {
            float healthPercent = (float)currentHealth / (float)maxHealth;
            float calculatedDots = healthPercent * totalDots;
            int truncatedDots = (int)calculatedDots;
            dotsToDisplay = Mathf.Clamp(truncatedDots, 1, maxHealth - 1);
        }

        for (int i = 0; i < dots.Count; i++)
        {
            Image dot = dots[i];

            dot.gameObject.SetActive(i < dotsToDisplay);
        }

        numDots = dotsToDisplay;

        RefreshDotColors();
    }

    public void RefreshDotColors()
    {
        if (numDots > greenThreshold)
        {
            SetAllDotSprites(green);
        }
        else if (numDots > yellowThreshold)
        {
            SetAllDotSprites(yellow);
        }
        else
        {
            SetAllDotSprites(red);
        }
    }

    public void SetAllDotSprites(Sprite sprite)
    {
        foreach (Image dot in dots)
        {
            dot.sprite = sprite;
            //dot.SetAllDirty();
        }
    }

    [ContextMenu("Flash White")]
    public void FlashWhite()
    {
        FlashDots(white, 0.1f);
    }

    public void FlashDots(Sprite sprite, float duration)
    {
        StartCoroutine(DoFlashDots(sprite, duration));
    }

    private IEnumerator DoFlashDots(Sprite sprite, float duration)
    {
        SetAllDotSprites(sprite);

        yield return new WaitForSeconds(duration);

        RefreshDotColors();
    }

	// Use this for initialization
	void Start () {
        CurrentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [ContextMenu("Test Health Bar")]
    public void TestHealthBar()
    {
        StartCoroutine(DoTestHealthBar());
    }

    private IEnumerator DoTestHealthBar()
    {
        int actualHealth = CurrentHealth;

        int testHealth = 0;

        while (testHealth <= maxHealth)
        {
            CurrentHealth = testHealth;

            testHealth++;

            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);

        testHealth = maxHealth;

        while (testHealth >= 0)
        {
            CurrentHealth = testHealth;

            testHealth--;

            yield return new WaitForSeconds(0.02f);
        }

        CurrentHealth = actualHealth;
    }

    public void SetHP(float currentHP)
    {
        if (currentHP > 0.0f)
        {
            CurrentHealth = Mathf.Clamp((int)currentHP, 1, maxHealth);
        }
        else
        {
            CurrentHealth = 0;
        }
    }
}
