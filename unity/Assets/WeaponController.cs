using UnityEngine;
using System;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour {

    public Animator primaryWeaponAnimator;
    public Animator secondaryWeaponAnimator;

    public List<BaseWeapon> primaryWeapons;
    public List<BaseWeapon> secondaryWeapons;
    public List<BaseWeapon> utilityWeapons;

    public int primaryWeaponIndex = 0;
    public int secondaryWeaponIndex = 0;
    public int utilityWeaponIndex = 0;

    public float weaponSelectionDelay = 0.5f;

    private Dictionary<WeaponLocation, WeaponSlot> weaponSlots;
    
	// Use this for initialization
	void Start () {
        weaponSlots = new Dictionary<WeaponLocation, WeaponSlot>();

        WeaponSlot slot = new WeaponSlot();

        slot.weaponLocation = WeaponLocation.Primary;
        slot.weaponAnimator = primaryWeaponAnimator;
        slot.currentWeaponIndex = primaryWeaponIndex;
        slot.cycledWeaponIndex = primaryWeaponIndex;
        slot.weaponList = primaryWeapons;

        weaponSlots.Add(WeaponLocation.Primary, slot);

        slot = new WeaponSlot();

        slot.weaponLocation = WeaponLocation.Secondary;
        slot.weaponAnimator = secondaryWeaponAnimator;
        slot.currentWeaponIndex = secondaryWeaponIndex;
        slot.cycledWeaponIndex = secondaryWeaponIndex;
        slot.weaponList = secondaryWeapons;

        weaponSlots.Add(WeaponLocation.Secondary, slot);

        slot = new WeaponSlot();

        slot.weaponLocation = WeaponLocation.Utility;
        slot.weaponAnimator = null;
        slot.currentWeaponIndex = utilityWeaponIndex;
        slot.cycledWeaponIndex = utilityWeaponIndex;
        slot.weaponList = utilityWeapons;

        weaponSlots.Add(WeaponLocation.Utility, new WeaponSlot());
	}
	
	// Update is called once per frame
	void Update () {
	    
        
	}

    public void CycleWeapon(WeaponLocation weaponLocation)
    {
        //initiate moving to next weapon
        //don't actually change until weapon gets locked in by delay

    }

    //public void ForceSelectWeapon(WeaponLocation weaponLocation)
    //{

    //}

    public void DoEquipNextWeapon(WeaponLocation weaponLocation)
    {
        switch (weaponLocation)
        {
            case WeaponLocation.Primary:
                break;
            case WeaponLocation.Secondary:
                break;
            case WeaponLocation.Utility:
                break;
            default:
                Debug.LogError("DoEquipNextWeapon(): Invalid WeaponLocation!");
                break;
        }

        
    }

    public void WeaponEquipped(WeaponLocation weaponLocation)
    {
        //weapon equip animation finished, weapon is ready to fire


    }

    public void ShootWeapon(WeaponLocation weaponLocation)
    {
        switch (weaponLocation)
        {
            case WeaponLocation.Primary:
                primaryWeapons[primaryWeaponIndex].Shoot();  
                break;
            case WeaponLocation.Secondary:
                secondaryWeapons[secondaryWeaponIndex].Shoot();
                break;
            case WeaponLocation.Utility:
                utilityWeapons[utilityWeaponIndex].Shoot();
                break;
            default:
                Debug.LogError("ShootWeapon(): Invalid WeaponLocation!");
                break;
        }

    }


    private class WeaponSlot
    {
        public WeaponLocation weaponLocation;

        public Animator weaponAnimator;

        public List<BaseWeapon> weaponList;

        public int currentWeaponIndex = 0;
        public int cycledWeaponIndex = 0;

        float weaponSelectTime = 0.0f;

        public void CycleWeapon()
        {
            cycledWeaponIndex++;
            if (cycledWeaponIndex >= weaponList.Count)
            {
                cycledWeaponIndex = 0;
            }

            weaponSelectTime = Time.time;
        }



    }
}
