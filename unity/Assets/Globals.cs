using UnityEngine;
using System.Collections;

public class Globals : Singleton<Globals> {

    public GameObject Player;
    public GameObject DynamicObjects;
    public WeaponController WeaponController;

    public bool SpawnEnemies = true;

    public const int THE_VOID_LAYER = 31;

	// Use this for initialization
	void Start () {
        base.Start();

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnEnemies = !SpawnEnemies;
        }
	}
}
