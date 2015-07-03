using UnityEngine;
using System.Collections;

public class Shotgun : BaseWeapon
{

    public GameObject bulletPrefab;
    public Rigidbody2D shooter;

    public int numBullets = 50;

    public Vector2 bulletMinForce = new Vector2(9, -8);
    public Vector2 bulletMaxForce = new Vector2(11, 8);

    public Vector2 shooterForce = new Vector2(-40000, 0);

    public float fireDelay = 0.6f;
    private float lastShot = 0.0f;

    public bool firing = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        firing = false;
    }

    void FixedUpdate()
    {
        if (firing && Time.fixedTime > lastShot + fireDelay)
        {
            //determine bullets to fire
            //int bulletsToFire = (int)(bulletsPerSecond * Time.deltaTime);

            //if (bulletsToFire == 0) bulletsToFire = 1;

            for (int i = 0; i < numBullets; i++)
            {
                FireBullet();
            }

            shooter.AddRelativeForce(shooterForce);

            lastShot = Time.fixedTime;
            firing = false;
        }
    }

    private void FireBullet()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);

        bullet.transform.parent = Globals.Instance.DynamicObjects.transform;

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

        //bulletRB.velocity = transform.forward * bulletSpeed;

        Vector2 bulletForce = new Vector2(Random.Range(bulletMinForce.x, bulletMaxForce.x), Random.Range(bulletMinForce.y, bulletMaxForce.y));

        bulletRB.AddRelativeForce(bulletForce);

        

    }

    public override void Shoot()
    {
        firing = true;
    }
}
