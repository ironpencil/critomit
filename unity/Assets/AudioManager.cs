using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager> {

    public AudioMixer masterMixer;

    public AudioSource arenaSource1;
    public AudioSource arenaSource2;
    public AudioSource lobbySource;
    public AudioSource sfxUnderwaterSource;
    public AudioSource sfxUnfilteredSource;
    public AudioSource sfx3DSource;

    private AudioSource currentArenaSource;
    private AudioSource nextArenaSource;

    private int nextClip;
    private double currentClipStopTime;
    private double nextClipStopTime;

    public List<AudioClip> arenaMusic;
    public AudioClip lobbyMusic;

    public AudioClip currentMusicClip;

    public float toArenaTransition = 2;
    public float toLobbyTransition = 2;

    private bool arenaMusicPlaying = false;

    public AudioMixerSnapshot arenaSnapshot;
    public AudioMixerSnapshot lobbySnapshot;

    public bool ignoreUnderwater = false;

    public float duckedMusicVolume = -15.0f;

    public enum AudioState
    {
        Lobby,
        Arena
    }

    public AudioState currentAudioState;

	// Use this for initialization
	public override void Start () {
        base.Start();

        if (this != null)
        {

            SetMasterVolume(-5.0f);

            if (Globals.Instance.currentState == GameState.Arena)
            {
                TransitionToArena();
            }
            else
            {
                TransitionToLobby();
            }            

        }
	}
	
	// Update is called once per frame
	void Update () {

        if (arenaMusicPlaying)
        {
            CheckSwitchArenaSources();
        }

        if (DebugLogger.DEBUG_MODE.Equals("DEBUG"))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ignoreUnderwater = !ignoreUnderwater;
            }
        }

        //DebugLogger.Log("Audio dspTime=" + AudioSettings.dspTime);
	}

    public void DuckMusic()
    {
        masterMixer.SetFloat("musicDucking", duckedMusicVolume);
    }

    public void UnduckMusic()
    {
        masterMixer.SetFloat("musicDucking", 0.0f);
    }

    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat("musicVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("sfxVol", volume);
    }

    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("masterVol", volume);
    }

    public float GetMusicVolume()
    {
        float volume = 0.0f;
        masterMixer.GetFloat("musicVol", out volume);
        return volume;
    }

    public float GetSFXVolume()
    {
        float volume = 0.0f;
        masterMixer.GetFloat("sfxVol", out volume);
        return volume;
    }

    public float GetMasterVolume()
    {
        float volume = 0.0f;
        masterMixer.GetFloat("masterVol", out volume);
        return volume;
    }

    [ContextMenu("Move to Arena")]
    public void TransitionToArena()
    {
        currentAudioState = AudioState.Arena;

        arenaSnapshot.TransitionTo(toArenaTransition);
        
        StartArenaMusic();

        StartCoroutine(StopLobbyMusicAfterSeconds(toArenaTransition, false));
        
        //arenaSource1.PlayScheduled(AudioSettings.dspTime + 10.0f);
        //DebugLogger.Log("Scheduling source to play at " + AudioSettings.dspTime + 10.0f);
        //if (arenaMusic.Count > 0)
        //{
        //    currentMusic = arenaMusic[Random.Range(0, arenaMusic.Count)];

            
        //}
    }

    [ContextMenu("Move to Lobby")]
    public void TransitionToLobby()
    {
        currentAudioState = AudioState.Lobby;

        lobbySnapshot.TransitionTo(toLobbyTransition);        

        StartLobbyMusic();

        StartCoroutine(StopArenaMusicAfterSeconds(toLobbyTransition, false));
    }

    private void StartLobbyMusic()
    {
        int randomClip = Random.Range(0, arenaMusic.Count);
        //int randomClip = 0; //for testing

        nextClip = randomClip;

        lobbySource.Stop();

        lobbySource.clip = lobbyMusic;

        currentMusicClip = lobbyMusic;

        lobbySource.Play();

    }

    [ContextMenu("Start Arena Music")]
    private void StartArenaMusic()
    {
        int randomClip = Random.Range(0, arenaMusic.Count);
        //int randomClip = 0; //for testing

        nextClip = randomClip;

        StopArenaMusic();

        currentArenaSource = arenaSource1;
        nextArenaSource = arenaSource2;

        currentArenaSource.clip = arenaMusic[randomClip];
        currentMusicClip = currentArenaSource.clip;
        currentArenaSource.Play();

        currentClipStopTime = AudioSettings.dspTime + currentMusicClip.length;

        arenaMusicPlaying = true;

        ScheduleNextArenaClip();

    }

    public void StopArenaMusic()
    {
        arenaSource1.Stop();
        arenaSource2.Stop();

        arenaMusicPlaying = false;
    }

    public IEnumerator StopArenaMusicAfterSeconds(float stopAfterSeconds, bool force)
    {
        yield return new WaitForSeconds(stopAfterSeconds);

        if (force || currentAudioState != AudioState.Arena)
        {
            StopArenaMusic();
        }
    }

    public IEnumerator StopLobbyMusicAfterSeconds(float stopAfterSeconds, bool force)
    {
        yield return new WaitForSeconds(stopAfterSeconds);

        if (force || currentAudioState != AudioState.Lobby)
        {
            lobbySource.Stop();
        }
    }

    private void ScheduleNextArenaClip()
    {
        nextClip++;

        if (nextClip >= arenaMusic.Count)
        {
            nextClip = 0;
        }

        float length = currentArenaSource.clip.length;
        double scheduleDelay = currentClipStopTime - AudioSettings.dspTime;
        
        nextArenaSource.clip = arenaMusic[nextClip];

        DebugLogger.Log("Scheduling clip. Length = " + length + " Delay = " + scheduleDelay);

        nextArenaSource.PlayScheduled(currentClipStopTime);
        nextClipStopTime = currentClipStopTime + nextArenaSource.clip.length;
    }   

    private void CheckSwitchArenaSources()
    {
        //if the current source stops, switch the other source to current
        //and schedule the next clip on this one
        if (!currentArenaSource.isPlaying)
        {
            DebugLogger.Log("Current AudioSource stopped playing. Switching sources.");
            AudioSource currentlyPlaying = nextArenaSource;
            nextArenaSource = currentArenaSource;
            currentArenaSource = currentlyPlaying;

            currentMusicClip = currentArenaSource.clip;

            currentClipStopTime = nextClipStopTime;

            ScheduleNextArenaClip();
        }
    }

    public void Play3DClipAtPoint(AudioClip clip, float vol, float pitch, Vector3 pos)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.spatialBlend = 1.0f;
        aSource.clip = clip; // define the clip
        aSource.volume = vol;
        aSource.pitch = pitch;
        aSource.outputAudioMixerGroup = sfx3DSource.outputAudioMixerGroup;
        aSource.rolloffMode = sfx3DSource.rolloffMode;
        aSource.minDistance = sfx3DSource.minDistance;
        aSource.maxDistance = sfx3DSource.maxDistance;
        aSource.priority = 256;
        // set other aSource properties here, if desired
        aSource.Play(); // start the sound
        Destroy(tempGO, clip.length); // destroy object after clip duration        
    }
}
