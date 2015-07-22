using UnityEngine;
using System.Collections;

public class GrappleHook : RemoteProjectile {

    private SpringJoint2D dummySpring;
    private DistanceJoint2D hook;

    private SpringJoint2D attachedSpring;
    private DistanceJoint2D attachedRope;

    public LineRenderer cable;

    public float attachTime = 5.0f;
    public float attachDistance = 0.0f;
    public bool isAttached = false;

    public float maxRangeSlack = 0.1f;

    public float attachFrequency = 0.0f;

    public bool useSpringJoint = false;

	// Use this for initialization
	public override void Start () {
        base.Start();
        dummySpring = GetComponent<SpringJoint2D>();
        hook = GetComponent<DistanceJoint2D>();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();

        if (isAttached)
        {
            //UpdateDistance();
        }
        UpdateCable();
	}

    protected override void CollideWithObject(Collision2D coll)
    {
        if (!isAttached)
        {
            try
            {
                RemoteLauncher launcher = (RemoteLauncher)shooterScript;
                launcher.refireOnManualActivation = false;
            }
            catch { }

            keepVelocityUpdated = false;
            //add distance script to hit object, connecting it to player

            //add spring script to hit object, connecting it to player

            isAttached = true;

            if (attachTime > 0)
            {
                die = Time.time + attachTime;
            }
            else
            {
                die = -1;
            }

            gameObject.transform.parent = coll.gameObject.transform;
                //attachedSpring = coll.gameObject.AddComponent<SpringJoint2D>();

                //attachedSpring.anchor = gameObject.transform.localPosition;

                //attachedSpring.connectedBody = shooter;

            bool makeKinematic = false;
            if (coll.gameObject.GetComponent<Rigidbody2D>() == null)
            {
                makeKinematic = true;
            }

            if (useSpringJoint)
            {
                attachedSpring = coll.gameObject.AddComponent<SpringJoint2D>();
                attachedSpring.enabled = false;
            }
            else
            {
                attachedRope = coll.gameObject.AddComponent<DistanceJoint2D>();
            }
            
            

            if (makeKinematic)
            {
                coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            }

            if (useSpringJoint)
            {
                attachedSpring.anchor = gameObject.transform.localPosition;
                attachedSpring.connectedBody = shooter;
                attachedSpring.frequency = dummySpring.frequency;
                attachedSpring.dampingRatio = dummySpring.dampingRatio;
                //attachedSpring.connectedAnchor = dummySpring.connectedAnchor;

                attachedSpring.enableCollision = true;
            }
            else
            {
                attachedRope.anchor = gameObject.transform.localPosition;
                attachedRope.connectedBody = shooter;
                attachedRope.maxDistanceOnly = true;
                attachedRope.enableCollision = true;
            }

            

            

            //hook.connectedBody = coll.gameObject.GetComponent<Rigidbody2D>();
            //hook.connectedAnchor = gameObject.transform.localPosition;

            //Destroy(GetComponent<Rigidbody2D>());
            
            //GetComponent<Rigidbody2D>().isKinematic = true; //lock this rigidbody in place

                //attachFrequency = dummySpring.frequency;
                //attachedSpring.enableCollision = true;

                //attachDistance = UpdateDistance();

                //attachedSpring.enabled = true;
            //hook.enabled = true;
            
            float distance = Vector2.Distance(shooter.transform.position, gameObject.transform.position);

            if (useSpringJoint)
            {
                attachedSpring.distance = distance;
                attachDistance = distance;
                //UpdateDistance();
            }
            else
            {

                attachedRope.distance = distance;

                attachedRope.enabled = true;
            }

            UpdateCable();

            Destroy(dummySpring);
            Destroy(hook);
            Destroy(GetComponent<Rigidbody2D>(), 0.001f);

            //below code is obsolete - used to parent this object to the target
            //gameObject.transform.parent = coll.gameObject.transform;
            //isAttached = true;

            //if (attachTime > 0)
            //{
            //    die = Time.time + attachTime;
            //}
            //else
            //{
            //    die = -1;
            //}

            //spring.connectedBody = shooter;

            //hook.connectedBody = coll.gameObject.GetComponent<Rigidbody2D>();
            //hook.connectedAnchor = gameObject.transform.localPosition;
            

            ////GetComponent<Rigidbody2D>().isKinematic = true; //lock the rigidbody in place
            
            //attachFrequency = spring.frequency;

            //attachDistance = UpdateDistance();

            //spring.enabled = true;
            //hook.enabled = true;

        }
        //SpringJoint2D otherSpring = coll.gameObject.AddComponent<SpringJoint2D>();

        //otherSpring


    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isAttached)
        {
            UpdateDistance();
            
        }
        UpdateCable();
    }

    public override void RemoteActivate()
    {
        base.RemoteActivate();
        Destroy(gameObject);              
    }

    void OnDestroy()
    {
        Destroy(attachedSpring);
        Destroy(attachedRope);

        try
        {
            RemoteLauncher launcher = (RemoteLauncher)shooterScript;
            launcher.refireOnManualActivation = true;
        }
        catch { }  

    }

    protected float UpdateDistance()
    {
        if (!useSpringJoint) { return 0.0f; }

        float currentDistance = Vector3.Distance(shooter.transform.position, gameObject.transform.position);

        if (currentDistance > (attachDistance + maxRangeSlack))
        {            
            //attachedSpring.distance = attachDistance;
            attachedSpring.enabled = true;
            //attachedSpring.frequency = attachFrequency;
        }
        else
        {
            attachedSpring.enabled = false; 
            //attachedSpring.distance = currentDistance;
            
            //attachedSpring.dampingRatio
            //attachedSpring.frequency = 0.0001f;
        }

        return currentDistance;
    }

    protected void UpdateCable()
    {        
        cable.SetPosition(0, gameObject.transform.position);
        cable.SetPosition(1, shooter.transform.position);
        cable.sortingOrder = -1;
    }
}
