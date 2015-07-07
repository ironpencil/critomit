using UnityEngine;

public class Gun : BaseWeapon
{
    public GameObject bulletPrefab;
    public Rigidbody2D shooter;

    public int bulletsPerShot = 1;

    public Vector2 bulletMinDirection = new Vector2(1.0f, 0.0f);
    public Vector2 bulletMaxDirection = new Vector2(1.0f, 0.0f);

    public Vector2 bulletMinMaxForce = new Vector2(5000.0f, 6000.0f);

    public Vector2 shooterForce = new Vector2(-40000, 0);    

    public bool autoFire = true;

    public float fireDelay = 0.6f;
    public bool varyFireDelay = false;
    public float fireDelayVariantRange = 0.01f;
    protected float actualFireDelayVariant = 0.0f;

    protected float lastShot = 0.0f;

    protected bool doShoot = false;

    protected bool didShoot = false;

    protected bool stoppedShooting = false;

    public virtual void Update()
    {
        if (!didShoot)
        {
            stoppedShooting = true;
        }

        doShoot = didShoot;
        didShoot = false;
    }

    public virtual void FixedUpdate()
    {
        if (doShoot)
        {            
            if (Time.fixedTime > lastShot + fireDelay + actualFireDelayVariant)
            {
                //if the weapon has auto mode or if they had previously stopped shooting
                if (autoFire || stoppedShooting)
                {

                    for (int i = 0; i < bulletsPerShot; i++)
                    {
                        Fire();
                    }

                    shooter.AddRelativeForce(shooterForce);

                    lastShot = Time.fixedTime;
                    if (varyFireDelay && fireDelayVariantRange > 0.0f)
                    {
                        actualFireDelayVariant = Random.Range(0.0f, fireDelayVariantRange);
                    }
                }
            }

            stoppedShooting = false;
        }

        doShoot = false;
    }

    public override void Shoot()
    {
        didShoot = true;
    }

    protected virtual BaseBullet Fire()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();

        bulletScript.shooter = shooter;

        bullet.transform.parent = Globals.Instance.DynamicObjects.transform;

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

        //bulletRB.velocity = transform.forward * bulletSpeed;

        Vector2 bulletDirection = new Vector2(Random.Range(bulletMinDirection.x, bulletMaxDirection.x), Random.Range(bulletMinDirection.y, bulletMaxDirection.y));

        float bulletSpeed = Random.Range(bulletMinMaxForce.x, bulletMinMaxForce.y);

        Vector2 bulletVelocity = bulletDirection * bulletSpeed;
        //bulletScript.Initialize(bulletVelocity);

        Vector2 previousVelocity = bulletRB.velocity;
        bulletRB.AddRelativeForce(bulletVelocity);

        return bulletScript;

        //Debug.Log("Fire() pv=" + previousVelocity + " v=" + bulletRB.velocity);
    }

    public override void SelectWeapon()
    {
        //when we switch weapons, set it up so that weapons without autoFire
        // think that we shot this frame so if the user is holding fire button
        // from previous autoFire weapon, they have to release and fire again
        if (!autoFire)
        {
            stoppedShooting = false;
            didShoot = true;
        }
    }
}
