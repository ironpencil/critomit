using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : Singleton<ScoreManager> {


    public int addMultiplierKills = 10;
    

    [SerializeField]
    private long currentWavePoints = 0;

    public long CurrentWavePoints
    {
        get { return currentWavePoints; }
        private set
        {
            currentWavePoints = value;
            UpdatePointsDisplay();
        }
    }

    [SerializeField]
    private long currentWaveMultiplier = 1;

    public long CurrentWaveMultiplier
    {
        get { return currentWaveMultiplier; }
        private set
        {
            currentWaveMultiplier = value;
            UpdateMultiplierDisplay();
        }
    }

    [SerializeField]
    private int currentKills = 0;

    public int CurrentKills
    {
        get { return currentKills; }
        private set
        {
            currentKills = value;
            while (currentKills >= addMultiplierKills)
            {
                currentKills -= addMultiplierKills;
                CurrentWaveMultiplier += 1;
                EventTextManager.Instance.AddEvent("!! MULTIPLIER GAINED !!", 1.0f, true);
            }
        }
    }
    
	// Use this for initialization
	public override void Start () {
        base.Start();

        UpdatePointsDisplay();
        UpdateMultiplierDisplay();
	}
	
	// Update is called once per frame
	void Update () {
        
	
	}

    public void AddPoints(long points, bool applyMultiplier)
    {
        if (applyMultiplier)
        {
            CurrentWavePoints += (points * currentWaveMultiplier);
        }
        else
        {
            CurrentWavePoints += points;
        }
    }

    public void AddKilledEnemyPoints(long points)
    {
        CurrentKills++;
        AddPoints(points, true);
    }

    public void ResetMultiplier()
    {
        if (CurrentWaveMultiplier > 1)
        {
            EventTextManager.Instance.AddEvent("!! MULTIPLIER LOST !!", 1.0f, true);
        }

        CurrentWaveMultiplier = 1;        
    }

    public void RefreshPointsDisplay()
    {
        UpdatePointsDisplay();
        UpdateMultiplierDisplay();
    }

    private void UpdatePointsDisplay() {
        ObjectManager.Instance.pointsText.text = currentWavePoints.ToString();
    }

    private void UpdateMultiplierDisplay() {
        ObjectManager.Instance.multiplierText.text = "X" + currentWaveMultiplier;
    }
}
