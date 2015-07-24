using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

    public Transform target;

    public float rotationPerSecond = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        if (target != null)
        {
            target.Rotate(new Vector3(0.0f, 0.0f, rotationPerSecond * Time.deltaTime));
        }
	
	}
}
