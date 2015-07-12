using UnityEngine;
using System.Collections.Generic;

public class WeaponArray : BaseWeapon
{
    public List<BaseWeapon> weapons = new List<BaseWeapon>();

    public virtual void Awake()
    {
        if (weaponDisplayer == null)
        {
            weaponDisplayer = gameObject.GetComponent<WeaponDisplayer>();
        }
    }

    public override void Shoot()
    {
        foreach (BaseWeapon weapon in weapons)
        {
            weapon.Shoot();
        }
    }

    public override void SelectWeapon()
    {
        foreach (BaseWeapon weapon in weapons)
        {
            weapon.SelectWeapon();
        }
    }
}
