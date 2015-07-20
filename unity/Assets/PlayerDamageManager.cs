using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDamageManager : TakesDamage {

    public List<SpriteRenderer> playerSprites = new List<SpriteRenderer>();

    public HealthBarManager healthBar;

    public void Start()
    {
        if (healthBar == null)
        {
            healthBar = ObjectManager.Instance.healthBar;
        }

        RefreshHealthbar();
    }

    private void RefreshHealthbar()
    {
        MaxHitPoints = MaxHitPoints;
        CurrentHP = CurrentHP;
    }


    public override int MaxHitPoints
    {
        get { return maxHitPoints; }
        set
        {
            maxHitPoints = value;
            healthBar.maxHealth = maxHitPoints;
        }
    }

    public override float CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = value;
            healthBar.SetHP(currentHP);
            markedForDeath = !(currentHP > 0.0f);
        }
    }

    public float damageInvDuration = 0.25f;

	// Update is called once per frame
	public override void Update () {

        if (markedForDeath)
        {
            //player died
            Debug.Log("Player is dead!");

            Globals.Instance.PlayerDied();

        }
	
	}

    [ContextMenu("Test Damage")]
    public void TestDamage()
    {
        ApplyDamage(1.0f, EffectSource.Universal, null);
    }

    public override bool ApplyDamage(float damage, EffectSource damageType, Collision2D coll)
    {
        //if we're already marked for death, don't apply more damage
        if (markedForDeath || invulnerable) { return false; }

        bool damageDealt = false;

        if (damagedBy == EffectSource.Universal ||
            damageType == EffectSource.Universal ||
            damagedBy == damageType)
        {
            damageDealt = true;
            DoDamage(damage);
        }

        return damageDealt;        
    }

    private void DoDamage(float damage)
    {
        CurrentHP = currentHP - damage;        

        if (!markedForDeath)
        {
            StartCoroutine(DoDamageEffects());
        }
        else
        {
            //kill player
            //for now, just restore to max health
            //CurrentHP = MaxHitPoints;
        }
    }

    private IEnumerator DoDamageEffects()
    {
        invulnerable = true;

        float endTime = Time.time + damageInvDuration;

        while (Time.time < endTime)
        {
            foreach (SpriteRenderer sprite in playerSprites)
            {
                if (sprite != null)
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a * 0.5f);
                }
            }

            yield return null;

            foreach (SpriteRenderer sprite in playerSprites)
            {
                if (sprite != null)
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a * 2.0f);
                }
            }

            yield return null;
        }

        invulnerable = false;
    }
}
