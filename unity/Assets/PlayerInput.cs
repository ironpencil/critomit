using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour {

    public List<BaseWeapon> primaryWeapons;
    public List<BaseWeapon> secondaryWeapons;

    public Vector2 currentVelocity = Vector2.zero;

    public Vector2 softMaxVelocity = new Vector2(15, 15);

    private Rigidbody2D rb;

    public GravityWell shield;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButton("Fire1"))
        {
            foreach (BaseWeapon weapon in primaryWeapons)
            {
                if (weapon != null)
                {
                    weapon.Shoot();
                }
            }            
        }

        if (Input.GetButton("Fire2"))
        {
            //Debug.Log("Fire2");
            foreach (BaseWeapon weapon in secondaryWeapons)
            {
                if (weapon != null)
                {
                    weapon.Shoot();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (shield != null)
            {
                shield.GravityActive = !shield.GravityActive;
            }
        }

        currentVelocity = rb.velocity;
	}

    void FixedUpdate()
    {
        Vector2 currVelocity = rb.velocity;
        float currMagnitude = currentVelocity.sqrMagnitude;

        float targetMagnitude = softMaxVelocity.sqrMagnitude;

        if (currMagnitude > targetMagnitude)
        {
            Vector2 slowDown = (targetMagnitude / currMagnitude) * rb.velocity;

            //rb.AddForce(slowDown);
            rb.AddForce(rb.velocity * -1, ForceMode2D.Impulse);
            //targetMagnitude = Mathf.Lerp(currMagnitude, targetMagnitude, Time.fixedDeltaTime);
            //rb.velocity = rb.velocity.normalized * targetMagnitude;
        }

        
                
        /*targetVelocity.x = Mathf.Ceil(softMaxVelocity.x);
        targetVelocity.y = Mathf.Ceil(softMaxVelocity.y);

        if (targetVelocity != rb.velocity)
        {
            //rb.velocity = Vector2.MoveTowards(rb.velocity, targetVelocity, Time.fixedDeltaTime);
            //rb.AddForce(targetVelocity - rb.velocity);
        }
         * */
    }
}
