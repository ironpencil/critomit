using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponDisplayManager : MonoBehaviour {

    public Image primaryWeaponImage;
    public Image secondaryWeaponImage;

    public void DisplayImage(WeaponLocation weaponLocation, Sprite sprite)
    {
        switch (weaponLocation)
        {
            case WeaponLocation.Primary:
                SetImage(primaryWeaponImage, sprite);
                break;
            case WeaponLocation.Secondary:
                SetImage(secondaryWeaponImage, sprite);
                break;
            case WeaponLocation.Utility:
                break;
            default:
                break;
        }
    }

    private void SetImage(Image weapon, Sprite sprite)
    {
        if (sprite == null)
        {
            weapon.sprite = null;
            weapon.enabled = false;
        }
        else
        {            
            weapon.sprite = sprite;
            weapon.enabled = true;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
