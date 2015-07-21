using UnityEngine;
using System.Collections;

public class MoveRandomlyTowardTarget : BaseMovement {

    public float varianceRange = 30.0f;
        
    public bool changeDirection = true;

    public void FixedUpdate()
    {
        if (Time.fixedTime > nextMovementTime)
        {
            changeDirection = true;
        }
    }

    public override void Move(Rigidbody2D movingRB, Rigidbody2D targetRB)
    {
        if (movingRB == null || targetRB == null) { return; }

        if (changeDirection)
        {
            RotateRandomlyTowardTarget(movingRB, targetRB);
            nextMovementTime = Time.fixedTime + movementDelay;
            changeDirection = false;
        }

        //if we're not at max speed, add force
        if (movingRB.velocity.sqrMagnitude < (maxVelocity * maxVelocity))
        {
            movingRB.AddRelativeForce(Vector2.right * accelerationForce);
        }
    }

    private void RotateRandomlyTowardTarget(Rigidbody2D movingRB, Rigidbody2D targetRB)
    {
        Vector3 targetPos = targetRB.transform.position;

        Vector3 thisPos = movingRB.transform.position;
        Vector2 offset = new Vector2(targetPos.x - thisPos.x, targetPos.y - thisPos.y);

        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        
        float random = Random.Range(0.0f, 1.0f);

        //float randomAngle = varianceRange * Mathf.Pow((2 * random - 1), 3.0f);

        float randomAngle = Random.Range(0.0f, varianceRange * 2) - varianceRange;

        movingRB.rotation = angle + randomAngle;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
