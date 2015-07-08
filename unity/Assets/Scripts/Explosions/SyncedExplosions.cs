using UnityEngine;
using System.Collections.Generic;

public class SyncedExplosions : TimedExplosion
{
    public List<SyncedExplosions> explosions = new List<SyncedExplosions>();

    public bool randomizeExplosionPositions = false;

    public Vector2 randomizedPositionMinRange = Vector2.zero;
    public Vector2 randomizedPositionMaxRange = Vector2.zero;
    
    public override void Start()
    {
        nextDamage = initialDamage;

        detonate = Time.time + delay;

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
            if (!initialDamageDone)
            {
                nextDamage = initialDamage;
                initialDamageDone = true;
            }
            else
            {
                //figure out damage over time tick
                nextDamage = damagePerSecond * Time.fixedDeltaTime;

            }
        }
        //Debug.Log("FixedUpdate(): initialDamageDone=" + preDamageDone + ". nextDamage=" + nextDamage + ". t=" + Time.fixedTime);
    }

    public virtual void Detonate()
    {
        detonated = true;
    }


}

