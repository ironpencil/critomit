using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundEffectHandler : MonoBehaviour {

    public List<AudioClip> clips;

    public bool playRandom;
    public bool underwater;

    public float volume = 1.0f;
    public bool variableVolume = true;    
    public float volumeMin = 0.9f;
    public float volumeMax = 1.0f;

    public float playDelay = 0.0f;
    public bool variableDelay = true;
    public float playDelayMin = 0.0f;
    public float playDelayMax = 0.01f;
    public bool ignoreTimeScale = false;

    public float pitch = 1.0f;
    public bool variablePitch = true;
    public float pitchMin = 0.75f;
    public float pitchMax = 1.25f;

    public bool playOneShot = true;    

    public void PlayEffect()
    {
        if (playRandom && clips.Count > 1)
        {
            int random = Random.Range(0, clips.Count);
            AudioClip clip = clips[random];
            PlayClip(clip);
        }
        else
        {
            foreach (AudioClip clip in clips)
            {
                PlayClip(clip);
            }            
        }
    }

    private void PlayClip(AudioClip clip)
    {
        float clipVolume;
        float clipPitch;
        float clipDelay;

        if (variableVolume)
        {
            clipVolume = Random.Range(volumeMin, volumeMax);
        }
        else
        {
            clipVolume = volume;
        }

        if (variablePitch)
        {
            clipPitch = Random.Range(pitchMin, pitchMax);
        }
        else
        {
            clipPitch = pitch;
        }

        if (variableDelay)
        {
            clipDelay = Random.Range(playDelayMin, playDelayMax);
        }
        else
        {
            clipDelay = playDelay;
        }

        if (playDelay > 0.0f)
        {
            StartCoroutine(WaitThenPlay(clip, clipVolume, clipPitch, clipDelay));
        }
        else
        {
            Play(clip, clipVolume, clipPitch);
        }
    }

    private void Play(AudioClip clip, float clipVolume, float clipPitch)
    {
        AudioSource source;

        if (underwater && !AudioManager.Instance.ignoreUnderwater)
        {
            source = AudioManager.Instance.sfxUnderwaterSource;
        }
        else
        {
            source = AudioManager.Instance.sfxUnfilteredSource;
        }

        source.pitch = clipPitch;

        if (playOneShot)
        {
            source.PlayOneShot(clip, clipVolume);
        }
        else
        {
            source.clip = clip;
            source.volume = clipVolume;
            source.Play();
        }

    }

    private IEnumerator WaitThenPlay(AudioClip clip, float clipVolume, float clipPitch, float clipDelay)
    {
        if (ignoreTimeScale)
        {
            float playTime = Time.realtimeSinceStartup + clipDelay;

            while (playTime > Time.realtimeSinceStartup)
            {
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(clipDelay);
        }

        Play(clip, clipVolume, clipPitch);
    }
}
