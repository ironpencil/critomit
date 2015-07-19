using UnityEngine;
using System.Collections;

public class ObjectManager : Singleton<ObjectManager> {

    public GameObject Player;
    public GameObject DynamicObjects;
    public WeaponController WeaponController;
    public CameraFollow FollowCam;

	// Use this for initialization
	void Start () {
        destroyOnLoad = true;
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
