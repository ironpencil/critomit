using UnityEngine;
using System.Collections;

public class TakesDamageChild : TakesDamage {

    public TakesDamage parentToDamage;

    public override bool MarkedForDeath
    {
        get { return parentToDamage.MarkedForDeath; }
        set { parentToDamage.MarkedForDeath = value; }
    }

    public override int MaxHitPoints
    {
        get { return parentToDamage.MaxHitPoints; }
        set { parentToDamage.MaxHitPoints = value; }
    }

    public override float CurrentHP
    {
        get { return parentToDamage.CurrentHP; }
        set { parentToDamage.CurrentHP = value; }
    }

    public override EffectSource DamagedBy
    {
        get { return parentToDamage.DamagedBy; }
        set { parentToDamage.DamagedBy = value; }
    }

    public override bool Invulnerable
    {
        get { return parentToDamage.Invulnerable; }
        set { parentToDamage.Invulnerable = value; }
    }

	// Use this for initialization
	public override void Start () {
	
	}

    public override void FixedUpdate()
    {
        
    }

    public override bool ApplyDamage(float damage, EffectSource damageType, Collision2D coll)
    {
        return parentToDamage.ApplyDamage(damage, damageType, coll);
    }

    
}
