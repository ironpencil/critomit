using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class EndWaveDialog : MonoBehaviour {

    public MessageBox endWaveMessageBox;    
    public Text endWaveText;

    public int bonusSingleLineThreshold = 8;
    public int maxDisplayedBonuses = 15;

    public bool earnedEndWaveBonus = true;
    public long endWaveBonus = 5000;

    public long healthLeftBonus = 10000;
    public long highestMultiplierBonus = 1000;

    public long enemiesKilledBonus = 100;



    public long totalBonusPoints = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PrepareDialog()
    {
        if (ArenaManager.Instance.wavesCompleted == 0)
        {
            // wtf? no waves completed? why are you here?

        }
        else
        {
            // show end wave summary            
            endWaveMessageBox.playOpenedSound = true;
            //endWaveMessageBox.duckMusicOnOpen = true;
            //endWaveMessageBox.unduckMusicOnClose = true;
            PerformScoreSummary();
            //introGroup.alpha = 0.0f;
            //mutatorGroup.alpha = 1.0f;
            //FlashImage mutatorFlash = mutatorGroup.GetComponent<FlashImage>();
            //mutatorFlash.doFlash = true;
        }
    }

    public void PerformScoreSummary()
    {
        List<string> bonusDescriptions = new List<string>();

        totalBonusPoints = 0;

        if (earnedEndWaveBonus)
        {
            bonusDescriptions.Add("WAVE COMPLETE: " + endWaveBonus);
            totalBonusPoints += endWaveBonus;
        }

        long remainingHPBonus = 0;

        PlayerDamageManager pdm = null;
        if (ObjectManager.Instance.player != null)
        {
            pdm = ObjectManager.Instance.player.GetComponent<PlayerDamageManager>();
        }

        if (pdm != null)
        {
            remainingHPBonus = (long) ((pdm.CurrentHP / pdm.MaxHitPoints) * healthLeftBonus);
        }

        if (remainingHPBonus > 0)
        {
            bonusDescriptions.Add("HEALTH REMAINING: " + remainingHPBonus);
            totalBonusPoints += remainingHPBonus;
        }

        long enemiesKilled = ScoreManager.Instance.totalWaveKills;

        long killedBonus = enemiesKilled * enemiesKilledBonus;

        bonusDescriptions.Add("ENEMIES KILLED: X" + enemiesKilled + " = " + killedBonus);
        totalBonusPoints += killedBonus;

        long highestMultiplier = ScoreManager.Instance.highestWaveMultiplier;

        long highestMultBonus = highestMultiplierBonus * highestMultiplier;

        bonusDescriptions.Add("HIGHEST MULTIPLIER: X" + highestMultiplier + " = " + highestMultBonus);
        totalBonusPoints += highestMultBonus;

        bonusDescriptions.Add("TOTAL WAVE BONUS: " + totalBonusPoints + "\r\nWAVE MULTIPLIER: X" + ArenaManager.Instance.wavesCompleted);

        totalBonusPoints = totalBonusPoints * ArenaManager.Instance.wavesCompleted;
        bonusDescriptions.Add("TOTAL: " + totalBonusPoints);

        bool doubleSpace = MutatorManager.Instance.pendingMutators.Count < bonusSingleLineThreshold;

        List<string> bonusText = new List<string>();

        bool addSpace = false;
        foreach (string bonusDescription in bonusDescriptions)
        {
            if (addSpace && doubleSpace)
            {
                bonusText.Add("");                
            }
            bonusText.Add(bonusDescription);

            addSpace = true;
        }

        int blankLines = maxDisplayedBonuses - bonusText.Count;
        int prependLines = (int)(blankLines * 0.5f);

        endWaveText.text = "";

        for (int i = 0; i < prependLines; i++)
        {
            endWaveText.text += "\r\n";
        }

        foreach (string description in bonusText)
        {
            endWaveText.text += description + "\r\n";
        }

    }

    public void ApplyTotalWaveBonus()
    {
        ScoreManager.Instance.AddPoints(totalBonusPoints, false);
    }

    public void EndWave()
    {
        //AudioManager.Instance.UnduckMusic();
        //ArenaManager.Instance.StartWave();
        ArenaManager.Instance.LoadNextLevel();
        endWaveMessageBox.StartClose();
        //Globals.Instance.Pause(false);
        Globals.Instance.acceptPlayerGameInput = true;
    }
}
