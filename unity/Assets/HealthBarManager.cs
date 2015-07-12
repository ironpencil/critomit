using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBarManager : MonoBehaviour {

    public List<Image> dots = new List<Image>();

    public int maxHealth = 100;

    [SerializeField]
    private int currentHealth;

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
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
}
