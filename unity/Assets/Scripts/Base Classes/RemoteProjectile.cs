using UnityEngine;
using System.Collections;

public class RemoteProjectile : BaseBullet {

    public bool remoteActivated = false;

    public bool activateOnDestroy = true;
   
	// Use this for initialization
	public virtual void Start () {
        base.Start();

	}
	
	// Update is called once per frame
    public virtual void Update()
    {
        base.Update();

	}

    public virtual void FixedUpdate()
    {
        base.FixedUpdate();

    }

    public virtual void OnDestroy()
    {
        if (activateOnDestroy)
        {
            RemoteActivate();
        }
    }

    protected override void CollideWithObject(Collision2D coll)
    {
        base.CollideWithObject(coll);
        RemoteActivate();
    }

    public virtual void RemoteActivate()
    {
        remoteActivated = true;
    }
}
