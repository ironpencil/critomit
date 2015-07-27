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
            bool playSound = true;
            while (currentKills >= addMultiplierKills)
            {
                currentKills -= addMultiplierKills;
                CurrentWaveMultiplier += 1;
                EventTextManager.Instance.AddEvent("!! MULTIPLIER GAINED !!", 1.0f, true);
                if (playSound && multiplierGainedSound != null)
                {
                    multiplierGainedSound.PlayEffect();
                    playSound = false;
                }
            }
        }
    }

    public SoundEffectHandler multiplierGainedSound;
    public SoundEffectHandler multiplierLostSound;
    
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
        int minimumMultiplier = ArenaManager.Instance.wavesCompleted + 1;

        if (CurrentWaveMultiplier > minimumMultiplier)
        {
            EventTextManager.Instance.AddEvent("!! MULTIPLIER LOST !!", 1.0f, true);
            if (multiplierLostSound != null)
            {
                multiplierLostSound.PlayEffect();
            }
        }

        CurrentWaveMultiplier = minimumMultiplier;        
    }

    public void VerifyMinimumMultiplier()
    {
        int minimumMultiplier = ArenaManager.Instance.wavesCompleted + 1;

        CurrentWaveMultiplier = minimumMultiplier;

        //or do we want to let them keep their multiplier between waves?
    }

    public void RefreshPointsDisplay()
    {
        UpdatePointsDisplay();
        UpdateMultiplierDisplay();
    }

    private void UpdatePointsDisplay() {
        if (ObjectManager.Instance.pointsText != null)
        {
            ObjectManager.Instance.pointsText.text = currentWavePoints.ToString();
        }
    }

    private void UpdateMultiplierDisplay() {
        if (ObjectManager.Instance.multiplierText != null)
        {
            ObjectManager.Instance.multiplierText.text = "X" + currentWaveMultiplier;
        }
    }
}
