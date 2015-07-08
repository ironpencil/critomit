using UnityEngine;

public class BaseExplosion : MonoBehaviour
{

    public float duration = 0.2f;
    protected float die = 0.0f;

    public float initialDamage = 10.0f;

    public float damagePerSecond = 10.0f;

    //protected float lastDamageTick = 0.0f;
    protected bool initialDamageDone = false;

    protected float nextDamage = 0.0f;

    public virtual void Awake()
    {
        nextDamage = initialDamage;

    }

    public virtual void Start()
    {
        nextDamage = initialDamage;

        if (duration > 0.0f)
        {
            die = Time.time + duration;
        }
    }

    public virtual void Update()
    {
        if (die > 0 && Time.time > die)
        {
            Destroy(gameObject);
        }
    }

    public virtual void FixedUpdate()
    {
        //bool preDamageDone = initialDamageDone;
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
        //Debug.Log("FixedUpdate(): initialDamageDone=" + preDamageDone + ". nextDamage=" + nextDamage + ". t=" + Time.fixedTime);
    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {
        //don't affect other trigger colliders
        if (!other.isTrigger)
        {
            //Debug.Log("!!!!!!!!!!OnTriggerStay2D() t=" + Time.fixedTime);
            CollideWithObject(other);            
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        //don't affect other trigger colliders
        if (!other.isTrigger)
        {
            //Debug.Log("!!!!!!!!!!OnTriggerEnter2D() t=" + Time.fixedTime);
            CollideWithObject(other);
        }
    }

    public virtual void CollideWithObject(Collider2D other)
    {
        initialDamageDone = true; //put this here also in case collision is the first thing that happens

        TakesDamage enemy = other.GetComponent<TakesDamage>();

        if (enemy != null)
        {
            bool destroyed = enemy.ApplyDamage(nextDamage);
            //Debug.Log("Applying " + nextDamage + " to " + enemy.name + ":" + enemy.GetInstanceID());
            //if (destroyed)
            //{
            //    Debug.Log("Destroyed!");
            //}
        }
    }


}

