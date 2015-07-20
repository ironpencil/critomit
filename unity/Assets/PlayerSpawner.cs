using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {    

    public bool healOnSpawn = true;

    public bool spawnOnStart = true;

    public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
        if (spawnOnStart)
        {
            DoSpawnPlayer();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DoSpawnPlayer()
    {        
        if (ObjectManager.Instance.player == null)
        {
            GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, transform.position, transform.rotation);
            ObjectManager.Instance.player = player;
            ObjectManager.Instance.followCam.followTarget = player.transform;
            ObjectManager.Instance.weaponController = player.GetComponent<WeaponController>();
        }
        else
        {
            ObjectManager.Instance.player.transform.position = transform.position;
            ObjectManager.Instance.player.transform.rotation = transform.rotation;
        }
    }
}
