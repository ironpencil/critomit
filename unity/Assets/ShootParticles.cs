using UnityEngine;
using System.Collections;

public class ShootParticles : MonoBehaviour {

    public ParticleSystem targetParticleSystem;

	// Use this for initialization
	void Start () {
        targetParticleSystem.Play();
        targetParticleSystem.enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            //particleSystem.Play();
            Debug.Log("starting emission");
            targetParticleSystem.enableEmission = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            //particleSystem.Stop();
            Debug.Log("stopping emission");
            targetParticleSystem.enableEmission = false;
        }
	}
}
