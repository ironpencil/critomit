using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMutatorHelper : MonoBehaviour {

    public TakesDamage takesDamage;
    public DamageOnTouch damageOnTouch;
    public List<BaseMovement> mutatedMovements;
    public CreateExplosionEffect deathExplosion;
    public LoopedSoundEffect loopedSoundNormal;
    public SpawnEnemyEffect splitEnemyEffect;
    public BaseExplosion enemyDeathDamageExplosion;

}
