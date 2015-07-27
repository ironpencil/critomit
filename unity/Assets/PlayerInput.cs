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

    public Vector2 currentVelocity = Vector2.zero;

    public Vector2 softMaxVelocity = new Vector2(15, 15);

    private Rigidbody2D rb;

    public GravityWell shield;

    public List<string> guiBlockedButtons = new List<string>();

    public bool doLimitVelocity = false;

    public LookAtMouse lookAtMouse;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        lookAtMouse = GetComponent<LookAtMouse>();
	}
	
	// Update is called once per frame
	void Update () {

        HandleGUIButtonBlocking();

        HandlePlayerInput();

        currentVelocity = rb.velocity;
	}

    private void HandleGUIButtonBlocking()
    {
        if (Input.GetButtonDown("Fire Front Weapon"))
        {
            if (GUIManager.IsMouseInputBlocked())
            {
                guiBlockedButtons.Add("Fire Front Weapon");
            }
        }

        if (Input.GetButtonUp("Fire Front Weapon"))
        {
            if (guiBlockedButtons.Contains("Fire Front Weapon"))
            {
                guiBlockedButtons.Remove("Fire Front Weapon");
            }
        }

        if (Input.GetButtonDown("Fire Rear Weapon"))
        {
            if (GUIManager.IsMouseInputBlocked())
            {
                guiBlockedButtons.Add("Fire Rear Weapon");
            }
        }

        if (Input.GetButtonUp("Fire Rear Weapon"))
        {
            if (guiBlockedButtons.Contains("Fire Rear Weapon"))
            {
                guiBlockedButtons.Remove("Fire Rear Weapon");
            }
        }
    }

    private void HandlePlayerInput()
    {
        if (!Globals.Instance.acceptPlayerGameInput) { return; }

        if (Input.GetButton("Fire Front Weapon") && !guiBlockedButtons.Contains("Fire Front Weapon"))
        {
            ObjectManager.Instance.weaponController.ShootWeapon(WeaponLocation.Primary);
        }

        if (Input.GetButton("Fire Rear Weapon") && !guiBlockedButtons.Contains("Fire Rear Weapon"))
        {
            ObjectManager.Instance.weaponController.ShootWeapon(WeaponLocation.Secondary);
        }

        if (Input.GetButton("Fire Special"))
        {
            ObjectManager.Instance.weaponController.ShootWeapon(WeaponLocation.Utility);
        }

        if (Input.GetButtonDown("Switch Front Weapon"))
        {
            ObjectManager.Instance.weaponController.CycleWeapon(WeaponLocation.Primary);

            /*primaryWeaponIndex++;
            if (primaryWeaponIndex >= primaryWeapons.Count)
            {
                primaryWeaponIndex = 0;
            }
            primaryWeapons[primaryWeaponIndex].SelectWeapon();
             * */

        }

        if (Input.GetButtonDown("Switch Rear Weapon"))
        {
            ObjectManager.Instance.weaponController.CycleWeapon(WeaponLocation.Secondary);

            //secondaryWeaponIndex++;
            //if (secondaryWeaponIndex >= secondaryWeapons.Count)
            //{
            //    secondaryWeaponIndex = 0;
            //}
            //secondaryWeapons[secondaryWeaponIndex].SelectWeapon();

        }

        if (Input.GetButtonDown("Switch Special"))
        {
            ObjectManager.Instance.weaponController.CycleWeapon(WeaponLocation.Utility);

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
            gameObject.GetComponent<PlayerDamageManager>().healthBar.TestHealthBar();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            gameObject.GetComponent<PlayerDamageManager>().healthBar.FlashWhite();
        }
    }

    void FixedUpdate()
    {

        if (Globals.Instance.acceptPlayerGameInput)
        {
            lookAtMouse.RotateWithPhysics();
        }

        //do we need to slow the player down?

        if (doLimitVelocity)
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
