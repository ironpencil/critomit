using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class StartWaveDialog : MonoBehaviour {

    public MessageBox startWaveMessageBox;
    public CanvasGroup introGroup;
    public CanvasGroup mutatorGroup;
    public Text mutatorDescriptionText;
    public int mutatorSingleLineThreshold = 8;
    public int maxDisplayedMutators = 15;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PrepareDialog()
    {
        if (ArenaManager.Instance.wavesCompleted == 0)
        {
            // show intro dialog
            mutatorGroup.alpha = 0.0f;
            introGroup.alpha = 1.0f;

        }
        else
        {
            // show mutator dialog 
            //AudioManager.Instance.DuckMusic();
            startWaveMessageBox.playOpenedSound = true;
            startWaveMessageBox.duckMusicOnOpen = true;
            startWaveMessageBox.unduckMusicOnClose = true;
            LoadMutatorDescriptions();
            introGroup.alpha = 0.0f;
            mutatorGroup.alpha = 1.0f;
            FlashImage mutatorFlash = mutatorGroup.GetComponent<FlashImage>();
            mutatorFlash.doFlash = true;
        }
    }

    public void LoadMutatorDescriptions()
    {
        bool doubleSpace = MutatorManager.Instance.pendingMutators.Count < mutatorSingleLineThreshold;
        List<string> mutatorDescriptions = new List<string>();

        bool addSpace = false;
        foreach (Mutator mutator in MutatorManager.Instance.pendingMutators)
        {
            //add double spacing, but do it before each new mutator and not for the first one
            if (doubleSpace && addSpace)
            {
                mutatorDescriptions.Add("");
            }

            string mutatorDesc = mutator.GetDescription();
            string fullDesc = mutator.mutatorName;
            
            if (mutatorDesc.Length > 0)
            {
                fullDesc += ":  " + mutatorDesc;
            }

            mutatorDescriptions.Add(fullDesc);

            addSpace = true;
        }

        int blankLines = maxDisplayedMutators - mutatorDescriptions.Count;
        int prependLines = (int)(blankLines * 0.5f);

        mutatorDescriptionText.text = "";

        for (int i = 0; i < prependLines; i++)
        {
            mutatorDescriptionText.text += "\r\n";
        }

        foreach (string description in mutatorDescriptions)
        {
            mutatorDescriptionText.text += description + "\r\n";
        }
        
    }
    
    
    public void StartWave()
    {
        //AudioManager.Instance.UnduckMusic();
        ArenaManager.Instance.StartWave();
        startWaveMessageBox.StartClose();
        //Globals.Instance.Pause(false);
        Globals.Instance.acceptPlayerGameInput = true;
    }

}
