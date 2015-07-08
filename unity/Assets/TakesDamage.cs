using UnityEngine;
using System.Collections;

public class TakesDamage : MonoBehaviour {

    public bool markedForDeath = false;

    public int maxHitPoints = 1;

    public float currentHP = 0.0f;

	// Use this for initialization
	void Start () {

        currentHP = maxHitPoints;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (markedForDeath)
        {
            DestroyImmediate(gameObject);
        }
	
	}

    public bool ApplyDamage(float damage)
    {
        //if we're already marked for death, don't apply more damage
        if (markedForDeath) { return false; }

        currentHP = currentHP - damage;

        markedForDeath = !(currentHP > 0.0f);

        return markedForDeath;        
    }
}
