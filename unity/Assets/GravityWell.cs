using UnityEngine;
using System.Collections;

public class GravityWell : MonoBehaviour {

    public float gravity = 100.0f;

    public float slowFactor = 0.95f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D otherRB = other.attachedRigidbody;

        if (otherRB != null)
        {
            Vector2 direction = transform.position - other.transform.position;

            float distance = Vector2.Distance(transform.position, other.transform.position);

            if (distance > 0.005f)
            {
                otherRB.AddForce(direction.normalized * gravity * otherRB.mass * otherRB.drag);
            }

            otherRB.velocity = otherRB.velocity * slowFactor;
        }
    }
}
