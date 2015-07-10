using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public Animator weaponAnimator;
    public RuntimeAnimatorController animationController;

    public abstract void Shoot();

    public abstract void SelectWeapon();
}
