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
    protected float privCurrentHP = 0.0f;

    public virtual float CurrentHP
    {
        get { return privCurrentHP; }
        set { privCurrentHP = Mathf.Min(value, maxHitPoints); }
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

    public List<GameEffect> damagedEffects = new List<GameEffect>();
    public List<GameEffect> deathEffects = new List<GameEffect>();

    public int pointValue = 100;
    public int killValue = 1;

    public float HPRegenAmount = 0.0f;
    public float HPRegenDelay = 1.0f;
    private float lastRegenTime = 0.0f;   

	// Use this for initialization
	public virtual void Start () {

        MaxHitPoints = maxHitPoints;
        CurrentHP = maxHitPoints;
	
	}
	
	// Update is called once per frame
	public virtual void Update () {

        if (markedForDeath)
        {
            ScoreManager.Instance.AddKilledEnemyPoints(pointValue, killValue);
            DestroyImmediate(gameObject);
        }
        else
        {
            if (HPRegenAmount > 0.0f)
            {
                if (Time.time > lastRegenTime + HPRegenDelay)
                {
                    CurrentHP += HPRegenAmount;
                    lastRegenTime = Time.time;
                }
            }

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
            privCurrentHP = privCurrentHP - damage;

            markedForDeath = !(privCurrentHP > 0.0f);

            foreach (GameEffect effect in damagedEffects)
            {
                effect.ActivateEffect(gameObject, damage);
            }

            if (markedForDeath)
            {
                foreach (GameEffect effect in deathEffects)
                {
                    effect.ActivateEffect(gameObject, damage);
                }

            }
        }

        return damageDealt;        
    }

    public virtual void Kill(bool forceVulnerable)
    {
        if (forceVulnerable)
        {
            invulnerable = false;
        }

        ApplyDamage(CurrentHP, EffectSource.Universal, null);
    }
}
