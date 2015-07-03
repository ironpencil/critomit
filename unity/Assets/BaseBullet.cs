using UnityEngine;
using System.Collections;

public class BaseBullet : MonoBehaviour {

    public float lifespan;

    public float die = -1.0f;

    public Rigidbody2D shooter;

	// Use this for initialization
	public virtual void Start () {
        if (lifespan > 0)
        {
            die = Time.time + lifespan;
        }
	}
	
	// Update is called once per frame
    public virtual void Update()
    {
        if (die > 0 && Time.time > die)
        {
            Destroy(this.gameObject);
        }
	}

    public virtual void OnCollisionEnter2D(Collision2D coll)
    {
        TakesDamage enemy = coll.gameObject.GetComponent<TakesDamage>();

        if (enemy != null)
        {
            Destroy(enemy.gameObject);
        }

        Destroy(this.gameObject);
    }


}
