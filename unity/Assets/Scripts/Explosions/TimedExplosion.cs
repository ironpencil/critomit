using UnityEngine;

public class TimedExplosion : BaseExplosion
{
    public float delay = 0.5f;
    protected float detonate = 0.0f;

    protected bool detonated = false;

    public override void Start()
    {
        nextDamage = initialDamage;

        detonate = Time.time + delay;

        //try to estimate when this will be destroyed
        if (duration > 0f)
        {
            die = detonate + duration;
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

        if (die > 0 && Time.time > die)
        {
            Destroy(gameObject);
        }
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
    }


}

