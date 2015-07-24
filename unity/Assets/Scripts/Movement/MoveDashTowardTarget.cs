﻿using UnityEngine;
using System.Collections;

public class MoveDashTowardTarget : BaseMovement {

    public bool isDashing = false;
    public bool canDash = true;

    public void FixedUpdate()
    {
        if (Time.fixedTime > nextMovementTime)
        {
            isDashing = false;
            canDash = true;
        }
    }

    public override void Move(Rigidbody2D movingRB, Rigidbody2D targetRB)
    {
        if (movingRB == null || targetRB == null) { return; }

        if (canDash)
        {
            RotateTowardTarget(movingRB, targetRB);

            //if we're not at max speed, add force
            if (movingRB.velocity.sqrMagnitude < (maxVelocity * maxVelocity))
            {
                movingRB.AddRelativeForce(Vector2.right * accelerationForce, ForceMode2D.Impulse);
                nextMovementTime = Time.fixedTime + movementDelay;
                isDashing = true;
                canDash = false;
            }
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