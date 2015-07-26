using UnityEngine;
using System.Collections;

public class StartWaveButton : MonoBehaviour {

    public void StartWave()
    {
        ArenaManager.Instance.StartWave();
        Globals.Instance.Pause(false);
    }
}
