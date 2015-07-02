using System;
using UnityEngine;

public class TopdownCharacter2D : MonoBehaviour
{
    [SerializeField]
    private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.

    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private Vector2 movement;

    private void Awake()
    {
        // Setting up references.
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        m_Rigidbody2D.velocity = movement;
    }


    public void Move(float moveX, float moveY)
    {


            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(moveX));

            // Move the character
            //m_Rigidbody2D.velocity = new Vector2(moveX * m_MaxSpeed, m_Rigidbody2D.velocity.y);
            movement = new Vector2(moveX * m_MaxSpeed, moveY * m_MaxSpeed);



    }

}