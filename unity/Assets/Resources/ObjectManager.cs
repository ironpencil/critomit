using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager : Singleton<ObjectManager> {

    public GameObject player;
    public WeaponController weaponController; 
    public GameObject dynamicObjects;
    public HealthBarManager healthBar;
    public WeaponDisplayManager weaponDisplayManager;
    public PlayerSpawner playerSpawner;
    public CameraFollow followCam;
    public List<MessageBox> autoMessageBoxes;
    public StartWaveDialog startWaveDialog;
    public EndWaveDialog endWaveDialog;
    public Text pointsText;
    public Text multiplierText;
    public Text waveText;
    public Text eventText;

	// Use this for initialization
    public override void Start()
    {
        destroyOnLoad = true;
        base.Start();

        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DestroyPlayer()
    {
        Destroy(player);
    }

    public void DestroyAndRespawnPlayer()
    {
        StartCoroutine(DoDestroyAndRespawnPlayer(2.0f));
    }

    private IEnumerator DoDestroyAndRespawnPlayer(float respawnDelay)
    {
        float respawnTime = Time.time + respawnDelay;

        Destroy(player);

        while (player != null || Time.time < respawnTime)
        {
            yield return null;
        }

        playerSpawner.DoSpawnPlayer();
    }
}
