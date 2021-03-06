﻿using UnityEngine;
using System.Collections;
using System.Text;

public class BaseBullet : MonoBehaviour {

    public float damage = 1.0f;

    public EffectSource damageType = EffectSource.Universal;

    public float lifespan;

    public float die = 0.0f;

    public Rigidbody2D shooter;
    public BaseWeapon shooterScript;

    protected Collider2D thisCollider;

    protected Rigidbody2D thisRB;

    protected Vector2 previousVelocity = Vector2.zero;
    protected Vector2 currentVelocity = Vector2.zero;

    protected bool initialized = false;
    protected bool keepVelocityUpdated = true;

    //private StringBuilder lifeStory = new StringBuilder();

    public virtual void Awake()
    {
        //thisCollider = GetComponent<Collider2D>();
        //thisRB = GetComponent<Rigidbody2D>();        
    }

    public virtual void Initialize(Vector2 velocity)
    {
        //DebugLogger.Log("Initializing v=" + velocity);
        currentVelocity = velocity;
        previousVelocity = velocity;
        initialized = true;
    }

	// Use this for initialization
	public virtual void Start () {
        if (lifespan > 0)
        {
            die = Time.time + lifespan;
        }

        thisCollider = GetComponent<Collider2D>();
        thisRB = GetComponent<Rigidbody2D>();  

       // lifeStory.AppendLine("Start()::v=" + thisRB.velocity);
	}
	
	// Update is called once per frame
    public virtual void Update()
    {
        //DebugLogger.Log("Update(): " + gameObject.GetInstanceID() + " v=" + thisRB.velocity);
        //lifeStory.AppendLine("Update()::v=" + thisRB.velocity);

        if (die > 0 && Time.time > die)
        {
            Destroy(gameObject);
        }
	}

    public virtual void FixedUpdate()
    {
        //DebugLogger.Log("FixedUpdate(): " + gameObject.GetInstanceID() + " v=" + thisRB.velocity);
        //lifeStory.AppendLine("FixedUpdate()::v=" + thisRB.velocity);

        if (!initialized)
        {
            //lifeStory.AppendLine("FixedUpdate()::Not initialized.");
            Initialize(thisRB.velocity);
            thisCollider.enabled = true;
        }
        else
        {
            if (keepVelocityUpdated)
            {
                previousVelocity = currentVelocity;
                currentVelocity = thisRB.velocity;
            }
        }
    }

    //use physics collision if this collider is a physics collider
    public virtual void OnCollisionEnter2D(Collision2D coll)
    {
        //DebugLogger.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\nOnCollisionEnter2D: " + gameObject.GetInstanceID() + " v=" + thisRB.velocity);

        //lifeStory.AppendLine("OnCollisionEnter2D()::v=" + thisRB.velocity);

        if (!thisCollider.isTrigger)
        {
            CollideWithObject(coll);
        }
    }

    protected virtual void CollideWithObject(Collision2D coll)
    {
        TakesDamage enemy = coll.gameObject.GetComponent<TakesDamage>();

        bool doDestroy = true;

        if (enemy != null)
        {
            if (enemy.MarkedForDeath)
            {
                this.thisRB.velocity = previousVelocity;
                doDestroy = false;
            }
            else
            {
                enemy.ApplyDamage(damage, damageType, coll);

                //enemy.markedForDeath = true;
                //other.attachedRigidbody.Sleep();
                //other.enabled = false;
                //other.gameObject.layer = Globals.THE_VOID_LAYER; //THE VOID CONSUMES
                //Destroy(enemy.gameObject);
            }
        }

        if (doDestroy)
        {
            Destroy(this.gameObject);
        }
    }

}
