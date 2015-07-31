using UnityEngine;
using System.Collections;

public class LoopedSoundEffect : MonoBehaviour {

    public SoundEffectHandler soundEffectHandler;

    public float loopDelayMin = 1.0f;
    public float loopDelayMax = 5.0f;

    public bool doLoopSound = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        StartCoroutine(PlayLoopedSound());
    }

    public IEnumerator PlayLoopedSound()
    {
        while (this.isActiveAndEnabled)
        {
            if (doLoopSound)
            {
                float randomDelay = Random.Range(loopDelayMin, loopDelayMax);

                yield return new WaitForSeconds(randomDelay);

                soundEffectHandler.PlayEffect();
            }

            yield return null;
        }
    }
}
