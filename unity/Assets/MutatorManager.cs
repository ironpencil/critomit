using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MutatorManager : Singleton<MutatorManager> {

    public List<Mutator> pendingMutators = new List<Mutator>();
    public List<Mutator> activeMutators = new List<Mutator>();
    public List<Mutator> allAvailableMutators = new List<Mutator>();

    public RotateObject cameraSpinner;

    public float chanceToAddNewMutator = 0.5f;
    public float mutatorChanceIncrementPerWave = 0.1f;

    public bool allowMultipleMutatorsPerWave = false;

    private bool initializeInUpdate = false;

    public void GenerateNewLevelMutators()
    {
        Debug.Log("MutatorManager::GenerateNewLevelMutators()");
        // roll random chance to see if any new mutators need applied
        // if they do, apply them

        //TODO: Need to adjust random check so that we apply the weight modification immediately
        //Also need to check to see        

        float mutatorChance = chanceToAddNewMutator + (mutatorChanceIncrementPerWave * ArenaManager.Instance.wavesCompleted);

        while (mutatorChance > 0.0f)
        {
            if (!(mutatorChance < Random.Range(0.0f, 1.0f)))
            {
                //select mutator and adjust its value/weight to indicate it has been selected
                Mutator randomMutator = SelectRandomAvailableMutator();
                if (randomMutator != null)
                {
                    IncrementMutatorValues(randomMutator);
                    
                    //we only add the mutator once, so only its final incremented value will be applied
                    if (!pendingMutators.Any(m => m.mutatorType == randomMutator.mutatorType))
                    {
                        pendingMutators.Add(randomMutator);
                    }
                }
            }

            if (allowMultipleMutatorsPerWave)
            {
                mutatorChance -= 1.0f;
            }
            else
            {
                mutatorChance = 0.0f; // could just break;
            }
        }

        //now that we have all of our selected mutators, reset the weights on all mutators

        //reset mutator weights
        //foreach (Mutator mutator in allAvailableMutators)
        //{
        //    mutator.currentWeight = mutator.initialWeight;
        //}

        //now add the mutators we selected randomly, which will also adjust their weights for next generation
        //foreach (Mutator mutator in pendingMutators)
        //{
        //    AddActiveMutator(mutator);
        //    IncrementMutatorValues(mutator);
        //}
    }

    public void ApplyPendingMutators()
    {
        foreach (Mutator mutator in pendingMutators)
        {
            AddActiveMutator(mutator);            
        }

        pendingMutators.Clear();
    }

    //private void AddRandomMutator()
    //{
    //    Mutator mutatorToAdd = SelectRandomAvailableMutator();

    //    AddActiveMutator(mutatorToAdd);
    //}

    private Mutator SelectRandomAvailableMutator()
    {
        
        float cumulativeWeight = 0.0f;

        foreach (Mutator mutator in allAvailableMutators)
        {
            if (mutator.allowedToApply)
            {
                mutator.cumulativeWeight = cumulativeWeight;
                cumulativeWeight += mutator.currentWeight;
            }
        }

        float randomValue = Random.Range(0.0f, cumulativeWeight);

        Mutator selectedMutator = allAvailableMutators.LastOrDefault(m => m.allowedToApply && !(m.cumulativeWeight > randomValue));

        return selectedMutator;
    }

    void Awake()
    {
        if (allAvailableMutators.Count == 0)
        {
            List<Mutator> childMutators = gameObject.GetComponentsInChildren<Mutator>(false).ToList();

            allAvailableMutators.AddRange(childMutators);
        }

        Reset();
    }

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

    void OnLevelWasLoaded(int level)
    {
        initializeInUpdate = true;
    }

    private void IncrementMutatorValues(Mutator mutator)
    {
        //adjust mutator's value and weight

        mutator.currentValue += mutator.incrementValue;

        if (mutator.currentValue > mutator.maxValue)
        {
            mutator.currentValue = mutator.maxValue;

            if (mutator.limitedApplication)
            {
                mutator.allowedToApply = false;
            }
        }

        //set mutator weight to decremented weight to decrease chance it is selected again this wave
        mutator.currentWeight = mutator.decrementedWeight;
    }
    
    private void AddActiveMutator(Mutator mutator)
    {
        //apply the mutator

        activeMutators.RemoveAll(m => m.mutatorType == mutator.mutatorType);

        activeMutators.Add(mutator);

        switch (mutator.mutatorType)
        {
            case MutatorType.Spin:
                EnableCameraSpinner(mutator.currentValue);
                break;
            default:
                break;
        }

        mutator.isCurrentlyActive = true;
    }

    //just removes the effect and marks inactive, doesn't remove from active list
    private void RemoveMutatorEffect(MutatorType mutatorName)
    {
        switch (mutatorName)
        {
            case MutatorType.Spin:
                DisableCameraSpinner();
                break;
            default:
                break;
        }

        activeMutators.FirstOrDefault(m => m.mutatorType == mutatorName).isCurrentlyActive = false;
    }

    //calls remove effect then removes from active mutators list
    private void RemoveActiveMutator(MutatorType mutatorName)
    {
        RemoveMutatorEffect(mutatorName);
        activeMutators.RemoveAll(m => m.mutatorType == mutatorName);
    }

    public void PauseActiveMutators()
    {
        foreach (Mutator mutator in activeMutators)
        {
            RemoveMutatorEffect(mutator.mutatorType);
        }
    }

    public void ResumeActiveMutators()
    {
        foreach (Mutator mutator in activeMutators)
        {
            AddActiveMutator(mutator);
        }
    }

    public void ClearActiveMutators()
    {
        foreach (Mutator mutator in activeMutators)
        {
            RemoveMutatorEffect(mutator.mutatorType);
        }

        activeMutators.Clear();
    }

    public void Reset()
    {
        ClearActiveMutators();

        foreach (Mutator mutator in allAvailableMutators)
        {
            ResetMutator(mutator);
        }
    }

    public void ResetMutator(Mutator mutator)
    {
        mutator.currentWeight = mutator.initialWeight;
        mutator.currentValue = mutator.initialValue;
        mutator.allowedToApply = true;
        mutator.cumulativeWeight = 0.0f;
    }

    [ContextMenu("Spin!")]
    public void SpinCamera()
    {
        if (activeMutators.Any(m => m.mutatorType == MutatorType.Spin))
        {
            RemoveActiveMutator(MutatorType.Spin);
        }
        else
        {
            AddActiveMutator(new Mutator { mutatorType = MutatorType.Spin, currentValue = 45.0f } );
        }
    }

    private void EnableCameraSpinner(float rotationSpeed)
    {
        if (cameraSpinner != null)
        {
            cameraSpinner.rotationPerSecond = rotationSpeed;
            cameraSpinner.doRotate = true;
        }
    }

    private void DisableCameraSpinner()
    {
        if (cameraSpinner != null)
        {
            cameraSpinner.doRotate = false;
            cameraSpinner.ResetRotation();
        }
    }

    public void MutateEnemy(GameObject enemy)
    {
        foreach (Mutator mutator in activeMutators)
        {
            switch (mutator.mutatorType)
            {
                case MutatorType.Spin:
                    break;
                case MutatorType.EnemySpeed:
                    foreach (BaseMovement movement in enemy.GetComponents<BaseMovement>())
                    {

                        movement.forceMultiplier = 1 + (mutator.currentValue * .01f);
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
