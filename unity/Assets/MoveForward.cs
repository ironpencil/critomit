using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour {

    public float accelerationForce = 50.0f;
    public float maxVelocity = 20.0f;

    public bool isMoving = false;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        isMoving = true;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate()
    {
        if (isMoving)
        {
            //if we're not at max speed, add force
            if (rb.velocity.sqrMagnitude < (maxVelocity * maxVelocity))
            {
                rb.AddRelativeForce(Vector2.right * accelerationForce);
            }
        }        

    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }
}
