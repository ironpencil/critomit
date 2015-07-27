using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager> {

    public AudioSource arenaSource1;
    public AudioSource arenaSource2;
    public AudioSource lobbySource;

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

	// Use this for initialization
	public override void Start () {
        base.Start();

        TransitionToLobby();
	}
	
	// Update is called once per frame
	void Update () {
        if (arenaMusicPlaying)
        {
            CheckSwitchArenaSources();
        }

        //Debug.Log("Audio dspTime=" + AudioSettings.dspTime);
	}

    [ContextMenu("Move to Arena")]
    public void TransitionToArena()
    {
        arenaSnapshot.TransitionTo(toArenaTransition);
        
        StartArenaMusic();

        StartCoroutine(StopLobbyMusicAfterSeconds(toArenaTransition));
        //arenaSource1.PlayScheduled(AudioSettings.dspTime + 10.0f);
        //Debug.Log("Scheduling source to play at " + AudioSettings.dspTime + 10.0f);
        //if (arenaMusic.Count > 0)
        //{
        //    currentMusic = arenaMusic[Random.Range(0, arenaMusic.Count)];

            
        //}
    }

    [ContextMenu("Move to Lobby")]
    public void TransitionToLobby()
    {
        lobbySnapshot.TransitionTo(toLobbyTransition);        

        StartLobbyMusic();

        StartCoroutine(StopArenaMusicAfterSeconds(toLobbyTransition));
    }

    private void StartLobbyMusic()
    {
        //int randomClip = Random.Range(0, arenaMusic.Count);
        int randomClip = 0; //for testing

        nextClip = randomClip;

        lobbySource.Stop();

        lobbySource.clip = lobbyMusic;

        currentMusicClip = lobbyMusic;

        lobbySource.Play();

    }

    [ContextMenu("Start Arena Music")]
    private void StartArenaMusic()
    {
        //int randomClip = Random.Range(0, arenaMusic.Count);
        int randomClip = 0; //for testing

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

    public IEnumerator StopArenaMusicAfterSeconds(float stopAfterSeconds)
    {
        yield return new WaitForSeconds(stopAfterSeconds);

        StopArenaMusic();
    }

    public IEnumerator StopLobbyMusicAfterSeconds(float stopAfterSeconds)
    {
        yield return new WaitForSeconds(stopAfterSeconds);

        lobbySource.Stop();
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

        Debug.Log("Scheduling clip. Length = " + length + " Delay = " + scheduleDelay);

        nextArenaSource.PlayScheduled(currentClipStopTime);
        nextClipStopTime = currentClipStopTime + nextArenaSource.clip.length;
    }

    private void CheckSwitchArenaSources()
    {
        //if the current source stops, switch the other source to current
        //and schedule the next clip on this one
        if (!currentArenaSource.isPlaying)
        {
            Debug.Log("Current AudioSource stopped playing. Switching sources.");
            AudioSource currentlyPlaying = nextArenaSource;
            nextArenaSource = currentArenaSource;
            currentArenaSource = currentlyPlaying;

            currentMusicClip = currentArenaSource.clip;

            currentClipStopTime = nextClipStopTime;

            ScheduleNextArenaClip();
        }
    }
}
