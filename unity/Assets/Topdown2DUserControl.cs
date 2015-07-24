using System;
using UnityEngine;

[RequireComponent(typeof(TopdownCharacter2D))]
public class Topdown2DUserControl : MonoBehaviour
{
    private TopdownCharacter2D m_Character;


    private void Awake()
    {
        m_Character = GetComponent<TopdownCharacter2D>();
    }


    private void Update()
    {
        // Read the inputs.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // Pass all parameters to the character control script.
        m_Character.Move(h, v);
    }


    private void FixedUpdate()
    {
        
    }
}
