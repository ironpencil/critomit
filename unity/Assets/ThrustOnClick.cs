using UnityEngine;
using System.Collections;

public class ThrustOnClick : MonoBehaviour {

    public bool doThrust = false;

    public Vector2 force = Vector2.left;    

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        doThrust = Input.GetButton("Fire1");        
	}

    void FixedUpdate()
    {
        if (doThrust)
        {
            rb.AddRelativeForce(force);
        }
    }
}
