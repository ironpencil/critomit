using UnityEngine;
using System.Collections;

public class BaseBomb : RemoteProjectile {

    public BaseExplosion explosionObject;
   
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

    public override void RemoteActivate()
    {
        if (!remoteActivated)
        {
            base.RemoteActivate();
            Destroy(gameObject);
            GameObject.Instantiate(explosionObject, gameObject.transform.position, gameObject.transform.rotation);
        }
    } 
}
