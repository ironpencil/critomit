using UnityEngine;
using System;
using System.Collections;

public class LookAtMouse : MonoBehaviour {

    public Vector2 TargetPos = Vector2.zero;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 thisPos = transform.localPosition;
        Vector2 offset = new Vector2(mousePos.x - thisPos.x, mousePos.y - thisPos.y);

        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
	}

    void FixedUpdate()
    {
        
    }
}
