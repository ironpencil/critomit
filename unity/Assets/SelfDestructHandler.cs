using UnityEngine;
using System.Collections;

public class SelfDestructHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DoSelfDestruct()
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

        Globals.Instance.acceptPlayerGameInput = true;

        PlayerDamageManager pdm = ObjectManager.Instance.player.GetComponent<PlayerDamageManager>();

        if (pdm != null)
        {
            pdm.Kill(true);
        }
    }
}
