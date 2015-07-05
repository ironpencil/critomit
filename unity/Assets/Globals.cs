﻿using UnityEngine;
using System.Collections;

public class Globals : Singleton<Globals> {

    public GameObject Player;
    public GameObject DynamicObjects;

    public bool SpawnEnemies = true;

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
