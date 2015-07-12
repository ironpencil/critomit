using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour {

    //public List<BaseWeapon> primaryWeapons;
    //public List<BaseWeapon> secondaryWeapons;
    //public List<BaseWeapon> utilityWeapons;    

    //public int primaryWeaponIndex = 0;
    //public int secondaryWeaponIndex = 0;
    //public int utilityWeaponIndex = 0;

    public HealthBarManager healthBar;

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
            Globals.Instance.WeaponController.ShootWeapon(WeaponLocation.Primary);
        }

        if (Input.GetButton("Fire2"))
        {
            Globals.Instance.WeaponController.ShootWeapon(WeaponLocation.Secondary);
        }

        if (Input.GetButton("Fire3"))
        {
            Globals.Instance.WeaponController.ShootWeapon(WeaponLocation.Utility);
        }

        if (Input.GetKeyDown("1"))
        {
            Globals.Instance.WeaponController.CycleWeapon(WeaponLocation.Primary);
            
            /*primaryWeaponIndex++;
            if (primaryWeaponIndex >= primaryWeapons.Count)
            {
                primaryWeaponIndex = 0;
            }
            primaryWeapons[primaryWeaponIndex].SelectWeapon();
             * */

        }

        if (Input.GetKeyDown("2"))
        {
            Globals.Instance.WeaponController.CycleWeapon(WeaponLocation.Secondary);

            //secondaryWeaponIndex++;
            //if (secondaryWeaponIndex >= secondaryWeapons.Count)
            //{
            //    secondaryWeaponIndex = 0;
            //}
            //secondaryWeapons[secondaryWeaponIndex].SelectWeapon();

        }

        if (Input.GetKeyDown("3"))
        {
            Globals.Instance.WeaponController.CycleWeapon(WeaponLocation.Utility);
            
            //utilityWeaponIndex++;
            //if (utilityWeaponIndex >= utilityWeapons.Count)
            //{
            //    utilityWeaponIndex = 0;
            //}
            //utilityWeapons[utilityWeaponIndex].SelectWeapon();

        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            rb.isKinematic = !rb.isKinematic;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //if (shield != null)
            //{
            //    shield.GravityActive = !shield.GravityActive;
            //}
            CameraShake shaker = Camera.main.GetComponent<CameraShake>();
            //shaker.AddShake(0.25f, 10.0f, 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            healthBar.TestHealthBar();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            healthBar.FlashWhite();
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
            //Vector2 slowDown = (targetMagnitude / currMagnitude) * rb.velocity;

            //rb.AddForce(slowDown);
            rb.AddForce(rb.velocity * -0.5f * rb.mass);
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
