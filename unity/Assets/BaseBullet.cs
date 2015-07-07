using UnityEngine;
using System.Collections;
using System.Text;

public class BaseBullet : MonoBehaviour {

    public float lifespan;

    public float die = 0.0f;

    public Rigidbody2D shooter;

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
        //Debug.Log("Initializing v=" + velocity);
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
        //Debug.Log("Update(): " + gameObject.GetInstanceID() + " v=" + thisRB.velocity);
        //lifeStory.AppendLine("Update()::v=" + thisRB.velocity);

        if (die > 0 && Time.time > die)
        {
            Destroy(this.gameObject);
        }
	}

    public virtual void FixedUpdate()
    {
        //Debug.Log("FixedUpdate(): " + gameObject.GetInstanceID() + " v=" + thisRB.velocity);
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
        //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\nOnCollisionEnter2D: " + gameObject.GetInstanceID() + " v=" + thisRB.velocity);

        //lifeStory.AppendLine("OnCollisionEnter2D()::v=" + thisRB.velocity);

        if (!thisCollider.isTrigger)
        {
            CollideWithObject(coll.collider);
        }
    }

    //use trigger collision if this collider is a trigger collider
    public virtual void OnTriggerEnter2D(Collider2D other)
    {        
        if (thisCollider.isTrigger)
        {
            //don't collide with other trigger colliders
            if (!other.isTrigger)
            {
                CollideWithObject(other);
            }
        }
    }

    protected virtual void CollideWithObject(Collider2D other)
    {
        TakesDamage enemy = other.gameObject.GetComponent<TakesDamage>();

        bool doDestroy = true;

        if (enemy != null)
        {
            if (enemy.markedForDeath)
            {
                this.thisRB.velocity = previousVelocity;
                doDestroy = false;
            }
            else
            {

                enemy.markedForDeath = true;
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
