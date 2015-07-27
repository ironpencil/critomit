using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaySoundEffect : GameEffect
{

    public SoundEffectHandler soundEffectHandler;

    public override void ActivateEffect(GameObject activator, float value)
    {
        if (soundEffectHandler != null)
        {
            soundEffectHandler.PlayEffect();
        }
    }

}    
