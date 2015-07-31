using UnityEngine;
using System.Collections;

public class QuitHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DoQuit()
    {
        try
        {
            StartWaveDialog swd = ObjectManager.Instance.startWaveDialog;

            if (swd != null)
            {
                if (swd.startWaveMessageBox.isOpen)
                {
                    swd.startWaveMessageBox.StartClose();
                }
            }

            EndWaveDialog ewd = ObjectManager.Instance.endWaveDialog;

            if (ewd != null)
            {
                if (ewd.endWaveMessageBox.isOpen)
                {
                    ewd.endWaveMessageBox.StartClose();
                }
            }

            Globals.Instance.acceptPlayerGameInput = false;
            Globals.Instance.Pause(false);

            PlayerDamageManager pdm = ObjectManager.Instance.player.GetComponent<PlayerDamageManager>();

            if (pdm != null)
            {
                pdm.Invulnerable = true;
            }

            Globals.Instance.DoQuit();
            
        } catch {

            Application.Quit();

        }
    }
}
