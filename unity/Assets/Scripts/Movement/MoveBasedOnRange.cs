using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MoveBasedOnRange : BaseMovement
{
    public float range = 8.0f;

    public BaseMovement inRangeMovement;
    public BaseMovement outOfRangeMovement;

    [SerializeField]
    private float currentDistance = 0.0f;

    public override void Move(Rigidbody2D movingRB, Rigidbody2D targetRB)
    {
        currentDistance = Vector2.Distance(targetRB.transform.position, movingRB.transform.position);

        if (currentDistance > range)
        {
            outOfRangeMovement.Move(movingRB, targetRB);
        }
        else
        {
            inRangeMovement.Move(movingRB, targetRB);
        }
    }
}
