using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{
    public float accelerationForce = 50.0f;
    public float maxVelocity = 20.0f;

    public float movementDelay = 0.0f;
    protected float nextMovementTime = 0.0f;

    public abstract void Move(Rigidbody2D movingRB, Rigidbody2D targetRB);
}
