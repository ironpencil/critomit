using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public Rigidbody2D targetRB;    

    public BaseMovement targetedMovement;
    public BaseMovement untargetedMovement;

    private Rigidbody2D thisRB;

    // Use this for initialization
    void Start()
    {
        thisRB = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetRB == null)
        {
            if (ObjectManager.Instance.player != null)
            {
                targetRB = ObjectManager.Instance.player.GetComponent<Rigidbody2D>();
            }
        }        
    }

    void FixedUpdate()
    {
        if (targetRB == null)
        {
            if (untargetedMovement != null)
            {
                untargetedMovement.Move(thisRB, targetRB);
            }
        }
        else
        {
            if (targetedMovement != null)
            {
                targetedMovement.Move(thisRB, targetRB);
            }
            //Vector3 targetPos = target.transform.position;

            //Vector3 thisPos = transform.localPosition;
            //Vector2 offset = new Vector2(targetPos.x - thisPos.x, targetPos.y - thisPos.y);

            //var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            //transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
