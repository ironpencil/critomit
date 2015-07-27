using UnityEngine;
using System.Collections;

public class CollisionSound : MonoBehaviour {

    public SoundEffectHandler soundEffectHandler;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (soundEffectHandler != null)
        {
            soundEffectHandler.PlayEffect();
        }
    }
}
