using UnityEngine;

public class BaseExplosion : MonoBehaviour
{

    public float duration = 0.2f;
    protected float die = 0.0f;

    protected bool initialized = false;

    //damage effects
    public bool doApplyDamage = true;
    public float initialDamage = 10.0f;
    public float damageOverTime = 5.0f;

    protected float nextDamage = 0.0f;

    protected float appliedDamageOverTime = 0.0f;
    protected float calculatedDamagePerSecond = 0.0f;

    protected bool doneApplyingDamage = false;


    //gravity effects
    public bool doApplyGravity = false;
    public float constantGravity = 0.0f;
    public float addlGravityOverTime = 0.0f;
    public float gravityDeadZoneRadius = 0.005f;    

    protected float appliedGravityOverTime = 0.0f;
    protected float calculatedGravityPerSecond = 0.0f;
    protected float currentTotalGravity = 0.0f;


    protected bool doneGrowingGravity = false;

    protected void CalculateDamageOverTime()
    {
        //damageOverTime is used as the total damage applied when duration is greater than 0
        //if duration is 0, meaning the effect lasts forever, damageOverTime is used as damage per second
        float dotDuration = 1.0f;

        if (duration > 0) {
            dotDuration = duration;
        }

        calculatedDamagePerSecond = damageOverTime / dotDuration;
    }

    protected void CalculateGravityOverTime()
    {
        //addlGravityOverTime is used as the total gravity applied when duration is greater than 0
        //if duration is 0, meaning the effect lasts forever, addlGravityOverTime is used as gravity per second
        float gotDuration = 1.0f;

        if (duration > 0)
        {
            gotDuration = duration;
        }

        calculatedGravityPerSecond = addlGravityOverTime / gotDuration;
    }

    public virtual void Awake()
    {
        nextDamage = initialDamage;
        currentTotalGravity = constantGravity;

    }

    public virtual void Start()
    {
        nextDamage = initialDamage;
        currentTotalGravity = constantGravity;

        CalculateDamageOverTime();
        CalculateGravityOverTime();

        if (duration > 0.0f)
        {
            die = Time.time + duration;
        }
    }

    public virtual void Update()
    {
        //only destroy if our duration timer is up AND we've applied all of our DOT
        if (die > 0 && Time.time > die &&
            (doneApplyingDamage || !doApplyDamage) && 
            (doneGrowingGravity || !doApplyGravity))
        {
            Destroy(gameObject);
        }
    }

    public virtual void FixedUpdate()
    {
        //bool preDamageDone = initialDamageDone;
        if (!initialized)
        {
            nextDamage = initialDamage;
            currentTotalGravity = constantGravity;
            initialized = true;
        }
        else
        {
            //figure out damage over time tick
            if (doApplyDamage)
            {
                float damageToApply = calculatedDamagePerSecond * Time.fixedDeltaTime;

                if (duration > 0)
                {
                    float remainingDamage = Mathf.Max(0.0f, damageOverTime - appliedDamageOverTime);
                    if (!(damageToApply < remainingDamage))
                    {
                        damageToApply = remainingDamage;
                        doneApplyingDamage = true;
                    }
                    appliedDamageOverTime += damageToApply;
                }

                //damage is done in bits and pieces, so we need to know how much we're doing this frame
                nextDamage = damageToApply;
            }

            if (doApplyGravity)
            {
                float gravityToApply = calculatedGravityPerSecond * Time.fixedDeltaTime;

                float sign = Mathf.Sign(gravityToApply);

                if (duration > 0)
                {
                    float remainingGravity = Mathf.Max(0.0f, Mathf.Abs(addlGravityOverTime - appliedGravityOverTime));

                    if (!(Mathf.Abs(gravityToApply) < remainingGravity))
                    {
                        gravityToApply = remainingGravity * sign;
                        doneGrowingGravity = true;
                    }
                    appliedGravityOverTime += gravityToApply;
                }
                
                //gravity applies the full, growing force every frame
                currentTotalGravity += gravityToApply;
            }
            
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
        initialized = true; //put this here also in case collision is the first thing that happens

        if (doApplyDamage && nextDamage > 0)
        {
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

        if (doApplyGravity)
        {
            SuffersGravityEffects gravityTarget = other.GetComponent<SuffersGravityEffects>();

            if (gravityTarget != null)
            {
                Rigidbody2D otherRB = other.attachedRigidbody;

                if (otherRB != null)
                {
                    Vector2 direction = transform.position - other.transform.position;

                    float distance = Vector2.Distance(transform.position, other.transform.position);

                    if (distance > gravityDeadZoneRadius)
                    {
                        otherRB.AddForce(direction.normalized * currentTotalGravity * otherRB.mass * otherRB.drag * gravityTarget.gravityFactor);
                    }

                    //otherRB.velocity = otherRB.velocity * slowFactor;
                }

            }
        }
    }


}

