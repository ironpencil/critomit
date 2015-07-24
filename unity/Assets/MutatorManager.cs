using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MutatorManager : Singleton<MutatorManager> {

    public Dictionary<MutatorName, float> activeMutators = new Dictionary<MutatorName, float>();

    public RotateObject cameraSpinner;

    private bool initializeInUpdate = false;

    void Start()
    {
        base.Start();

        if (cameraSpinner == null)
        {
            cameraSpinner = ObjectManager.Instance.followCam.GetComponentInChildren<RotateObject>();
        }

        if (cameraSpinner == null)
        {
            initializeInUpdate = true;
        }

    }

    void Update()
    {
        if (initializeInUpdate)
        {
            initializeInUpdate = false;
            if (cameraSpinner == null)
            {
                cameraSpinner = ObjectManager.Instance.followCam.GetComponentInChildren<RotateObject>();
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SpinCamera();
        }


    }

    void OnLevelWasLoaded()
    {
        initializeInUpdate = true;
        ClearAllMutators();
    }

    public void ApplyMutator(MutatorName mutatorName, float mutatorValue)
    {
        switch (mutatorName)
        {
            case MutatorName.Spin:
                EnableCameraSpinner(mutatorValue);
                break;
            default:
                break;
        }

        activeMutators[mutatorName] = mutatorValue;
    }

    public void RemoveMutator(MutatorName mutatorName)
    {
        switch (mutatorName)
        {
            case MutatorName.Spin:
                DisableCameraSpinner();
                break;
            default:
                break;
        }

        if (activeMutators.ContainsKey(mutatorName))
        {
            activeMutators.Remove(mutatorName);
        }
    }

    public void ClearAllMutators()
    {
        activeMutators.Clear();

        cameraSpinner.enabled = false;
    }

    [ContextMenu("Spin!")]
    public void SpinCamera()
    {
        if (activeMutators.ContainsKey(MutatorName.Spin))
        {
            RemoveMutator(MutatorName.Spin);
        }
        else
        {
            ApplyMutator(MutatorName.Spin, 45.0f);
        }
    }

    private void EnableCameraSpinner(float rotationSpeed)
    {
        if (cameraSpinner != null)
        {
            cameraSpinner.enabled = true;
            cameraSpinner.rotationPerSecond = rotationSpeed;
        }
    }

    private void DisableCameraSpinner()
    {
        if (cameraSpinner != null)
        {
            cameraSpinner.enabled = false;
        }
    }

}
