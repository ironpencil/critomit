using UnityEngine;
using System.Collections;

public class MoveForward : BaseMovement {

    public override void Move(Rigidbody2D movingRB, Rigidbody2D targetRB)
    {
        if (movingRB == null || targetRB == null) { return; }

        //if we're not at max speed, add force
        if (movingRB.velocity.sqrMagnitude < (maxVelocity * maxVelocity))
        {
            movingRB.AddRelativeForce(Vector2.right * accelerationForce);
        }
    }
}
