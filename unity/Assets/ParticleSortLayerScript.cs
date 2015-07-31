using UnityEngine;
using System.Collections;

public class ParticleSortLayerScript : MonoBehaviour {

    public string sortingLayerName = "Explosions";

    public ParticleSystemRenderer psr;
	// Use this for initialization
	void Start () {
        if (psr != null)
        {
            psr.sortingLayerName = sortingLayerName;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
