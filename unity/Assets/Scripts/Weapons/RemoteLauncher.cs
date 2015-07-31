using UnityEngine;
using System.Collections;

public class RemoteLauncher : Gun
{    

    //public float detachDelay = 0.2f;
    //private float lastDetach = 0.0f;    

    public RemoteProjectile remoteProjectile;

    public bool refireOnManualActivation = false;

    public override void FixedUpdate()
    {
        if (doShoot)
        {
            if (remoteProjectile == null)
            {
                DoFireWeapon();
            }
            else
            {
                if (stoppedShooting) {
                    remoteProjectile.RemoteActivate();
                    remoteProjectile = null;

                    if (refireOnManualActivation)
                    {
                        DoFireWeapon();
                    }
                    //GameObject.Destroy(remoteProjectile.gameObject);
                    //remoteProjectile = null;
                }
            }

            stoppedShooting = false;
        }

        doShoot = false;


        //if (firing) 
        //{
        //    if (hook == null)
        //    {
        //        if (Time.fixedTime > lastShot + fireDelay &&
        //            Time.fixedTime > lastDetach + detachDelay)
        //        {

        //            //determine bullets to fire
        //            //int bulletsToFire = (int)(bulletsPerSecond * Time.deltaTime);

        //            //if (bulletsToFire == 0) bulletsToFire = 1;

        //            Fire();

        //            shooter.AddRelativeForce(shooterForce);

        //            lastShot = Time.fixedTime;
        //            firing = false;
        //        }
        //        else
        //        {
        //            firing = false;
        //        }
        //    }
        //    else
        //    {
        //        if (Time.fixedTime > lastShot + detachDelay)
        //        {
        //            GameObject.Destroy(hook.gameObject);
        //            hook = null;
        //            lastDetach = Time.fixedTime;
        //        }
        //        else
        //        {
        //            firing = false;
        //        }
        //    }
        //}
    }

    private void DoFireWeapon()
    {
        if (Time.fixedTime > lastShot + fireDelay + actualFireDelayVariant)
        {
            //if the weapon has auto mode or if they had previously stopped shooting
            if (autoFire || stoppedShooting)
            {

                for (int i = 0; i < bulletsPerShot; i++)
                {
                    remoteProjectile = (RemoteProjectile)Fire();
                }

                if (soundEffectHandler != null)
                {
                    soundEffectHandler.PlayEffect();
                }

                shooter.AddRelativeForce(shooterForce);

                if (particleSystem != null)
                {
                    particleSystem.Emit(Random.Range((int)minMaxParticles.x, (int)minMaxParticles.y + 1));
                }  

                if (cameraShaker != null)
                {
                    cameraShaker.Shake();
                }

                lastShot = Time.fixedTime;
                if (varyFireDelay && fireDelayVariantRange > 0.0f)
                {
                    actualFireDelayVariant = Random.Range(0.0f, fireDelayVariantRange);
                }
            }
        }
    }

    //private void FireBullet()
    //{

    //    GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);

    //    bullet.transform.parent = Globals.Instance.DynamicObjects.transform;

    //    Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

    //    //bulletRB.velocity = transform.forward * bulletSpeed;

    //    Vector2 bulletForce = new Vector2(Random.Range(bulletMinForce.x, bulletMaxForce.x), Random.Range(bulletMinForce.y, bulletMaxForce.y));

    //    bulletRB.AddRelativeForce(bulletForce);

    //    GrappleHook bulletScript = bullet.GetComponent<GrappleHook>();

    //    bulletScript.shooter = shooter;

    //    hook = bulletScript;

    //}

    //public override void Shoot()
    //{
    //    firing = true;
    //}
}
