using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TakesDamage : MonoBehaviour {

    [SerializeField]
    protected bool markedForDeath = false;

    public virtual bool MarkedForDeath
    {
        get { return markedForDeath; }
        set { markedForDeath = value; }
    }

    [SerializeField]
    protected int maxHitPoints = 1;

    public virtual int MaxHitPoints
    {
        get { return maxHitPoints; }
        set { maxHitPoints = value; }
    }

    [SerializeField]
    protected float currentHP = 0.0f;

    public virtual float CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }

    [SerializeField]
    protected EffectSource damagedBy = EffectSource.Universal;

    public virtual EffectSource DamagedBy
    {
        get { return damagedBy; }
        set { damagedBy = value; }
    }

    [SerializeField]
    protected bool invulnerable = false;

    public virtual bool Invulnerable
    {
        get { return invulnerable; }
        set { invulnerable = value; }
    }

    public List<DamagedEffect> damagedEffects = new List<DamagedEffect>();

	// Use this for initialization
	public virtual void Start () {

        MaxHitPoints = maxHitPoints;
        CurrentHP = maxHitPoints;
	
	}
	
	// Update is called once per frame
	public virtual void Update () {

        if (markedForDeath)
        {
            DestroyImmediate(gameObject);
        }
	
	}

    public virtual bool ApplyDamage(float damage, EffectSource damageType, Collision2D coll)
    {
        //if we're already marked for death, don't apply more damage
        if (markedForDeath || invulnerable) { return false; }

        bool damageDealt = false;

        if (damagedBy == EffectSource.Universal ||
            damageType == EffectSource.Universal ||
            damagedBy == damageType)
        {
            damageDealt = true;
            currentHP = currentHP - damage;

            markedForDeath = !(currentHP > 0.0f);

            foreach (DamagedEffect effect in damagedEffects)
            {
                effect.Damaged(damage);
            }
        }

        return damageDealt;        
    }
}
