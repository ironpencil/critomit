using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class GUIManager : Singleton<GUIManager> {

    private const int IGNORE_RAYCAST_LAYER = 2;

	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {

        
	}

    public static bool IsMouseInputBlocked()
    {
        bool blockMouseInput = false;

        bool mouseOverGameObject = EventSystem.current.IsPointerOverGameObject();
        
        if (mouseOverGameObject)
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

            if (selectedObject != null && selectedObject.layer != IGNORE_RAYCAST_LAYER)
            {
                blockMouseInput = true;
            }    
        }

        return blockMouseInput;
    }
}
