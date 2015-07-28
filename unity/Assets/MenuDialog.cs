﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuDialog : MonoBehaviour {

    public MessageBox menuMessageBox;


    public ButtonSelectionHandler menuButton;
    public Text menuButtonText;

	// Use this for initialization
	void Start () {
        if (menuMessageBox == null)
        {
            menuMessageBox = gameObject.GetComponent<MessageBox>();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ToggleDisplayed()
    {
        if (menuMessageBox.isResizing)
        {
            //can't toggle right now
        }
        else
        {
            if (menuMessageBox.isOpen)
            {
                StartCoroutine(DoClose());
            }
            else
            {
                StartCoroutine(DoOpen());
            }
        }
    }

    private IEnumerator DoOpen()
    {
        Globals.Instance.Pause(true);
        menuButton.button.interactable = false;

        menuMessageBox.StartOpen();

        while (menuMessageBox.isResizing)
        {
            yield return null;
        }

        menuButton.onClickSound = menuButton.cancelSound;
        menuButton.button.interactable = true;
        menuButtonText.text = "CLOSE";
    }

    private IEnumerator DoClose()
    {
        menuButton.button.interactable = false;

        menuMessageBox.StartClose();

        while (menuMessageBox.isResizing)
        {
            yield return null;
        }

        menuButton.onClickSound = menuButton.confirmSound;
        menuButton.button.interactable = true;
        menuButtonText.text = "MENU";
        Globals.Instance.Pause(false);
    }
}