using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MoveStraightToTarget : BaseMovement
{
    public override void Move(Rigidbody2D movingRB, Rigidbody2D targetRB)
    {
        if (movingRB == null || targetRB == null) { return; }

        RotateTowardTarget(movingRB, targetRB);

        //if we're not at max speed, add force
        if (movingRB.velocity.sqrMagnitude < (maxVelocity * maxVelocity))
        {
            movingRB.AddRelativeForce(Vector2.right * accelerationForce);
        }
    }

    private void RotateTowardTarget(Rigidbody2D movingRB, Rigidbody2D targetRB)
    {        
        Vector3 targetPos = targetRB.transform.position;

        Vector3 thisPos = movingRB.transform.position;
        Vector2 offset = new Vector2(targetPos.x - thisPos.x, targetPos.y - thisPos.y);

        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        movingRB.rotation = angle;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
