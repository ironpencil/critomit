using UnityEngine;
using System.Collections.Generic;

public class WeaponArray : BaseWeapon
{
    public List<BaseWeapon> weapons = new List<BaseWeapon>();

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
