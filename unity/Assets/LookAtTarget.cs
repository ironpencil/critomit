using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour {

    public Transform target;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 targetPos = target.transform.position;

            Vector3 thisPos = transform.localPosition;
            Vector2 offset = new Vector2(targetPos.x - thisPos.x, targetPos.y - thisPos.y);

            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void FixedUpdate()
    {

    }
}
