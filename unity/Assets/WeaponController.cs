using UnityEngine;
using System;
using System.Collections;
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

    public float noAnimationEquipDelay = 0.25f;

    public WeaponDisplayManager displayManager;

    private Dictionary<WeaponLocation, WeaponSlot> weaponSlots;
    
	// Use this for initialization
	void Start () {
        weaponSlots = new Dictionary<WeaponLocation, WeaponSlot>();

        WeaponSlot slot = new WeaponSlot();

        slot.weaponLocation = WeaponLocation.Primary;
        slot.displayManager = displayManager;
        slot.weaponAnimator = primaryWeaponAnimator;
        slot.equippedWeaponIndex = primaryWeaponIndex;
        slot.cycledWeaponIndex = primaryWeaponIndex;
        slot.weaponSelectionDelay = weaponSelectionDelay;
        slot.weaponList = primaryWeapons;

        slot.Initialize();

        weaponSlots.Add(WeaponLocation.Primary, slot);

        slot = new WeaponSlot();

        slot.weaponLocation = WeaponLocation.Secondary;
        slot.displayManager = displayManager;
        slot.weaponAnimator = secondaryWeaponAnimator;
        slot.equippedWeaponIndex = secondaryWeaponIndex;
        slot.cycledWeaponIndex = secondaryWeaponIndex;
        slot.weaponSelectionDelay = weaponSelectionDelay;
        slot.weaponList = secondaryWeapons;

        slot.Initialize();

        weaponSlots.Add(WeaponLocation.Secondary, slot);

        slot = new WeaponSlot();

        slot.weaponLocation = WeaponLocation.Utility;
        slot.displayManager = displayManager;
        slot.weaponAnimator = null;
        slot.equippedWeaponIndex = utilityWeaponIndex;
        slot.cycledWeaponIndex = utilityWeaponIndex;
        slot.weaponSelectionDelay = weaponSelectionDelay;
        slot.weaponList = utilityWeapons;

        slot.Initialize();

        weaponSlots.Add(WeaponLocation.Utility, slot);

        //EquipCurrentWeapon(WeaponLocation.Primary);
        //EquipCurrentWeapon(WeaponLocation.Secondary);
        //EquipCurrentWeapon(WeaponLocation.Utility);
	}
	
	// Update is called once per frame
	void Update () {
        
        foreach (WeaponSlot slot in weaponSlots.Values)
        {
            if (slot.CheckChangingWeapons())
            {
                if (!slot.ChangeWeapons())
                {
                    //if this slot does not have an animator, use a generic delay then continue equipping
                    StartCoroutine(WaitAndUnequip(noAnimationEquipDelay, slot.weaponLocation));
                }
            }
        }
        
	}

    public void ShootWeapon(WeaponLocation weaponLocation)
    {
        WeaponSlot slot;

        if (weaponSlots.TryGetValue(weaponLocation, out slot))
        {
            slot.ShootWeapon();
        }
        else
        {
            Debug.LogError("ShootWeapon(): Invalid WeaponLocation!");
        }

    }

    public void CycleWeapon(WeaponLocation weaponLocation)
    {
        //initiate moving to next weapon
        //don't actually change until weapon gets locked in by delay

        WeaponSlot slot;

        if (weaponSlots.TryGetValue(weaponLocation, out slot))
        {
            slot.CycleWeapon();
        }
        else
        {
            Debug.LogError("CycleWeapon(): Invalid WeaponLocation!");
        }
    }

    //public void ForceSelectWeapon(WeaponLocation weaponLocation)
    //{

    //}

    public void WeaponUnequipped(WeaponLocation weaponLocation)
    {
        EquipCurrentWeapon(weaponLocation);
        
    }    

    public void WeaponEquipped(WeaponLocation weaponLocation)
    {
        //weapon equip animation finished, weapon is ready to fire

        WeaponSlot slot;

        if (weaponSlots.TryGetValue(weaponLocation, out slot))
        {
            slot.FinishEquipWeapon();


        }
        else
        {
            Debug.LogError("WeaponEquipped(): Invalid WeaponLocation!");
        }

    }

    public void EquipCurrentWeapon(WeaponLocation weaponLocation)
    {
        WeaponSlot slot;

        if (weaponSlots.TryGetValue(weaponLocation, out slot))
        {
            if (!slot.StartEquipWeapon())
            {
                //weapon has no animator, wait for generic delay then finish equipping
                StartCoroutine(WaitAndEquip(noAnimationEquipDelay, slot.weaponLocation));
            }
        }
        else
        {
            Debug.LogError("EquipCurrentWeapon(): Invalid WeaponLocation!");
        }
    }

    //public void UnequipCurrentWeapon(WeaponLocation weaponLocation)
    //{
    //    WeaponSlot slot;

    //    if (weaponSlots.TryGetValue(weaponLocation, out slot))
    //    {
    //        if (!slot.ChangeWeapons())
    //        {
    //            //if this slot does not have an animator, use a generic delay then continue equipping
    //            StartCoroutine(WaitAndUnequip(noAnimationEquipDelay, slot.weaponLocation));
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("UnequipCurrentWeapon(): Invalid WeaponLocation!");
    //    }
    //}

    IEnumerator WaitAndUnequip(float waitTime, WeaponLocation weaponLocation)
    {
        yield return new WaitForSeconds(waitTime);

        WeaponUnequipped(weaponLocation);
    }

    IEnumerator WaitAndEquip(float waitTime, WeaponLocation weaponLocation)
    {
        yield return new WaitForSeconds(waitTime);

        WeaponEquipped(weaponLocation);
    }

    private class WeaponSlot
    {
        public WeaponLocation weaponLocation;

        public WeaponDisplayManager displayManager;

        public Animator weaponAnimator;

        public List<BaseWeapon> weaponList;

        public BaseWeapon currentlyEquippedWeapon;
        public BaseWeapon currentlyCycledWeapon;

        public int equippedWeaponIndex = 0;
        public int cycledWeaponIndex = 0;

        float weaponSelectTime = 0.0f;
        public float weaponSelectionDelay = 0.0f;

        //public bool enabled = true;
        public bool cyclingWeapon = false;
        public bool weaponReady = false;

        public void Initialize()
        {
            ForceEquipWeapon(equippedWeaponIndex);
        }

        private void ForceEquipWeapon(int weaponIndex)
        {
            equippedWeaponIndex = weaponIndex;
            cycledWeaponIndex = equippedWeaponIndex;

            StartEquipWeapon();
            FinishEquipWeapon();
            DisplayEnabledImage(currentlyEquippedWeapon);
        }

        public bool CanShoot
        {
            get { return weaponReady; }
        }

        public void ShootWeapon()
        {
            if (CanShoot)
            {
                currentlyEquippedWeapon.Shoot();
            }
        }

        public void CycleWeapon()
        {
            CycleToWeapon(cycledWeaponIndex + 1, false);
        }

        public void CycleToWeapon(int index, bool forceReload)
        {
            cycledWeaponIndex = index;

            if (cycledWeaponIndex >= weaponList.Count)
            {
                cycledWeaponIndex = 0;
            }

            currentlyCycledWeapon = weaponList[cycledWeaponIndex];

            if (cycledWeaponIndex == equippedWeaponIndex && !forceReload)
            {
                //we cycled back to our current weapon
                cyclingWeapon = false;

                //if (CanShoot)
                //{
                DisplayEnabledImage(currentlyCycledWeapon);
                //}
                //else
                //{
                //    displayManager.SetImage(weaponLocation, currentlyEquippedWeapon.weaponDisplayer.disabledImage);
                //}
            }
            else
            {
                DisplayDisabledImage(currentlyCycledWeapon);

                cyclingWeapon = true;
                weaponSelectTime = Time.time + weaponSelectionDelay;
            }
        }

        public bool CheckChangingWeapons()
        {
            //check to see if we are changing weapons
            if (cyclingWeapon)
            {                
                //if it's time to lock in the current weapon selection, do so
                if (Time.time > weaponSelectTime)
                {
                    cyclingWeapon = false;

                    //if we're actually switching to a new weapon and not just re-selecting the equipped one
                    if (equippedWeaponIndex != cycledWeaponIndex)
                    {
                        weaponReady = false;
                        return true; //return true that we are changing weapons
                    }
                }
            }

            return false;

        }

        public bool ChangeWeapons()
        {
            bool hasAnimator = false;

            equippedWeaponIndex = cycledWeaponIndex;

            DisplayEnabledImage(currentlyCycledWeapon);

            if (weaponAnimator != null)
            {
                if (currentlyEquippedWeapon.animationController != null)
                {
                    weaponAnimator.SetTrigger(AnimationParams.SWITCH_WEAPONS);
                    hasAnimator = true;
                }
                else
                {
                    weaponAnimator.runtimeAnimatorController = null;
                }
            }

            return hasAnimator;
        }

        public bool StartEquipWeapon()
        {
            bool hasAnimator = false;

            currentlyEquippedWeapon = weaponList[equippedWeaponIndex];

            //DisplayEnabledImage(currentlyEquippedWeapon);

            //switch animation controllers
            if (weaponAnimator != null)
            {
                if (currentlyEquippedWeapon.animationController != null)
                {
                    weaponAnimator.runtimeAnimatorController = currentlyEquippedWeapon.animationController;
                    hasAnimator = true;
                }
                else
                {
                    weaponAnimator.runtimeAnimatorController = null;
                }
            }

            //do we need to start the animation or does it start automatically?

            //weaponAnimator.StartPlayback();

            return hasAnimator;
        }

        public void FinishEquipWeapon()
        {
            currentlyEquippedWeapon.SelectWeapon();
            weaponReady = true;
        }

        private void DisplayDisabledImage(BaseWeapon weapon)
        {
            WeaponDisplayer displayer = weapon.weaponDisplayer;

            if (displayer == null)
            {
                displayManager.DisplayImage(weaponLocation, null);
            }
            else
            {
                displayManager.DisplayImage(weaponLocation, displayer.disabledImage);
            }
        }

        private void DisplayEnabledImage(BaseWeapon weapon)
        {
            WeaponDisplayer displayer = weapon.weaponDisplayer;

            if (displayer == null)
            {
                displayManager.DisplayImage(weaponLocation, null);
            }
            else
            {
                displayManager.DisplayImage(weaponLocation, displayer.enabledImage);
            }
        }

    }
}
