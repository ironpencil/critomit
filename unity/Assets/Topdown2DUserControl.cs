using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        // Pass all parameters to the character control script.
        m_Character.Move(h, v);
    }


    private void FixedUpdate()
    {
        
    }
}
