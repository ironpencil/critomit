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
    public long endWaveBonus = 10000;

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
            bonusDescriptions.Add("WAVE COMPLETE BONUS: " + endWaveBonus);
            totalBonusPoints += endWaveBonus;
        }



        bool doubleSpace = MutatorManager.Instance.pendingMutators.Count < bonusSingleLineThreshold;

        List<string> bonusText = new List<string>();

        foreach (string bonusDescription in bonusDescriptions)
        {
            bonusText.Add(bonusDescription);
            if (doubleSpace)
            {
                bonusText.Add("");
            }
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
