using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CameraToggle : MonoBehaviour {

    public enum CamOption
    {
        Rotation,
        Bounce
    }

    public CamOption camOption;

    public Toggle camToggle;
    public Text optionText;

    public string onText;
    public string offText;

	// Use this for initialization
	void Start () {
        if (camToggle == null)
        {
            camToggle = gameObject.GetComponent<Toggle>();
        }
	
	}

    public void AdjustCamera()
    {
        //set option on or off
        switch (camOption)
        {
            case CamOption.Rotation:
                Globals.Instance.cameraSpinEnabled = camToggle.isOn;
                if (camToggle.isOn)
                {                    
                    MutatorManager.Instance.MenuEnableCameraRotation();
                }
                else
                {
                    MutatorManager.Instance.MenuDisableCameraRotation();
                }

                break;
            case CamOption.Bounce:
                break;
            default:
                break;
        }

        AdjustOptionText();
    }

    public void AdjustOptionText()
    {
        if (camToggle.isOn) {
            optionText.text = onText;
        }
        else
        {
            optionText.text = offText;
        }
    }
}
