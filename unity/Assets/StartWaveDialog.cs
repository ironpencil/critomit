using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StartWaveDialog : MonoBehaviour {

    public CanvasGroup introGroup;
    public CanvasGroup mutatorGroup;
    public Text mutatorDescriptionText;
    public int mutatorSingleLineThreshold = 8;
    public int maxDisplayedMutators = 15;

    public MutatorDisplayHandler mutatorDisplay;

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
            mutatorDisplay.playAlarm = true;
            LoadMutatorDescriptions();
            introGroup.alpha = 0.0f;
            mutatorGroup.alpha = 1.0f;
        }
    }

    public void LoadMutatorDescriptions()
    {
        bool doubleSpace = MutatorManager.Instance.pendingMutators.Count < mutatorSingleLineThreshold;
        List<string> mutatorDescriptions = new List<string>();
        foreach (Mutator mutator in MutatorManager.Instance.pendingMutators)
        {
            mutatorDescriptions.Add(mutator.mutatorName + ": " + mutator.GetDescription());
            if (doubleSpace)
            {
                mutatorDescriptions.Add("");
            }
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
        ArenaManager.Instance.StartWave();
        //Globals.Instance.Pause(false);
        Globals.Instance.acceptPlayerGameInput = true;
    }

}
