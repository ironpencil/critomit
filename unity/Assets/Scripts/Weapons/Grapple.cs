using UnityEngine;
using System.Collections;

public class Grapple : BaseWeapon
{

    public GameObject bulletPrefab;
    public Rigidbody2D shooter;

    public Vector2 bulletMinForce = new Vector2(8, 0);
    public Vector2 bulletMaxForce = new Vector2(8, 0);

    public Vector2 shooterForce = new Vector2(0, 0);

    public float fireDelay = 1.0f;
    public float detachDelay = 0.2f;
    private float lastShot = 0.0f;
    private float lastDetach = 0.0f;

    public bool firing = false;

    public GrappleHook hook;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //firing = false;
    }

    void FixedUpdate()
    {
        if (firing) 
        {
            if (hook == null)
            {
                if (Time.fixedTime > lastShot + fireDelay &&
                    Time.fixedTime > lastDetach + detachDelay)
                {

                    //determine bullets to fire
                    //int bulletsToFire = (int)(bulletsPerSecond * Time.deltaTime);

                    //if (bulletsToFire == 0) bulletsToFire = 1;

                    FireBullet();

                    shooter.AddRelativeForce(shooterForce);

                    lastShot = Time.fixedTime;
                    firing = false;
                }
                else
                {
                    firing = false;
                }
            }
            else
            {
                if (Time.fixedTime > lastShot + detachDelay)
                {
                    GameObject.Destroy(hook.gameObject);
                    hook = null;
                    lastDetach = Time.fixedTime;
                }
                else
                {
                    firing = false;
                }
            }
        }
    }

    private void FireBullet()
    {

        GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

        //bulletRB.velocity = transform.forward * bulletSpeed;

        Vector2 bulletForce = new Vector2(Random.Range(bulletMinForce.x, bulletMaxForce.x), Random.Range(bulletMinForce.y, bulletMaxForce.y));

        bulletRB.AddRelativeForce(bulletForce);

        GrappleHook bulletScript = bullet.GetComponent<GrappleHook>();

        bulletScript.shooter = shooter;

        hook = bulletScript;

    }

    public override void Shoot()
    {
        firing = true;
    }
}
