using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MoveWaveToTarget : BaseMovement
{
    public float magnitude = 1.0f;

    public float frequency = 4.0f;

    public bool randomSeed = true;

    private float timeSeed = 0.0f;

    public void Start()
    {
        if (randomSeed)
        {
            timeSeed = UnityEngine.Random.Range(0.0f, 100.0f);
        }
    }

    public override void Move(Rigidbody2D movingRB, Rigidbody2D targetRB)
    {
        if (movingRB == null || targetRB == null) { return; }

        RotateTowardTarget(movingRB, targetRB);

        //if we're not at max speed, add force
        if (movingRB.velocity.sqrMagnitude < (maxVelocity * maxVelocity * forceMultiplier))
        {
            movingRB.AddRelativeForce(Vector2.right * accelerationForce * forceMultiplier);
        }
    }

    private void RotateTowardTarget(Rigidbody2D movingRB, Rigidbody2D targetRB)
    {        
        Vector3 targetPos = targetRB.transform.position;

        Vector3 thisPos = movingRB.transform.position;
        Vector2 offset = new Vector2(targetPos.x - thisPos.x, targetPos.y - thisPos.y);

        //var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        var angle = Mathf.Atan2(offset.y, offset.x) + (magnitude * Mathf.Sin((Time.fixedTime + timeSeed) * frequency));

        var deg = angle * Mathf.Rad2Deg;

        movingRB.rotation = deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
