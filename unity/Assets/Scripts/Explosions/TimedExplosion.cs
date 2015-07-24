using UnityEngine;

public class TimedExplosion : BaseExplosion
{
    public float delay = 0.5f;

    public GameObject detonationObject;

    protected float detonate = 0.0f;

    protected bool detonated = false;

    public override void Start()
    {
        detonate = Time.time + delay;

        if (detonationObject != null)
        {
            detonationObject.SetActive(false);
        }

        base.Start();

        

        //try to estimate when this will be destroyed
        if (duration > 0f)
        {
            die += detonate;
        }
    }

    public override void Update()
    {
        if (!detonated && Time.time > detonate)
        {
            Detonate();

            //resync destruction with actual time instead of using estimate
            if (duration > 0f)
            {
                die = Time.time + duration;
            }
        }

        base.Update();
    }

    public virtual void FixedUpdate()
    {
        //bool preDamageDone = initialDamageDone;
        if (detonated)
        {
            base.FixedUpdate();
        }
        //Debug.Log("FixedUpdate(): initialDamageDone=" + preDamageDone + ". nextDamage=" + nextDamage + ". t=" + Time.fixedTime);
    }

    public override void CollideWithObject(Collider2D other)
    {
        if (detonated)
        {
            base.CollideWithObject(other);
        }
    }

    public virtual void Detonate()
    {
        detonated = true;

        if (detonationObject != null)
        {
            detonationObject.SetActive(true);
        }
    }


}

