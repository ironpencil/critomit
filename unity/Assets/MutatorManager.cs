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
        DebugLogger.Log("MutatorManager::GenerateNewLevelMutators()");
        // roll random chance to see if any new mutators need applied
        // save the selected mutators to be applied when the wave starts              

        if (!Globals.Instance.cameraSpinEnabled)
        {
            Mutator spinMutator = allAvailableMutators.FirstOrDefault(m => m.mutatorType == MutatorType.Spin);

            if (spinMutator != null)
            {
                spinMutator.allowedToApply = false;
            }
        }

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

        if (DebugLogger.DEBUG_MODE.Equals("DEBUG"))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SpinCamera();
            }
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
            case MutatorType.SpawnTime:
                float newSpawnTime = SpawnManager.Instance.initialSpawnTimer - mutator.currentValue;
                SpawnManager.Instance.spawnTimer = newSpawnTime;
                break;
            case MutatorType.TilesetSwap:
                if (ObjectManager.Instance.tileSetSwitcher != null)
                {
                    ObjectManager.Instance.tileSetSwitcher.doSwitch = true;
                }
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
            case MutatorType.SpawnTime:
                SpawnManager.Instance.spawnTimer = SpawnManager.Instance.initialSpawnTimer;
                break;
            case MutatorType.TilesetSwap:
                if (ObjectManager.Instance.tileSetSwitcher != null)
                {
                    ObjectManager.Instance.tileSetSwitcher.doSwitch = false;
                }
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

    public void MenuEnableCameraRotation()
    {
        Mutator spinMutator = activeMutators.FirstOrDefault(m => m.mutatorType == MutatorType.Spin);

        if (spinMutator != null)
        {
            EnableCameraSpinner(spinMutator.currentValue);
        }
    }

    public void MenuDisableCameraRotation()
    {
        Mutator spinMutator = activeMutators.FirstOrDefault(m => m.mutatorType == MutatorType.Spin);

        if (spinMutator != null)
        {
            DisableCameraSpinner();
        }
    }

    public void MutateEnemy(GameObject enemy)
    {
        EnemyMutatorHelper mutHelper = enemy.GetComponent<EnemyMutatorHelper>();
        if (mutHelper == null) { return; }

        foreach (Mutator mutator in activeMutators)
        {
            switch (mutator.mutatorType)
            {
                case MutatorType.None:
                    break;
                case MutatorType.Spin:
                    break;
                case MutatorType.EnemySpeed:
                    if (mutHelper.mutatedMovements != null)
                    {
                        foreach (BaseMovement movement in mutHelper.mutatedMovements)
                        {
                            movement.forceMultiplier = 1 + (mutator.currentValue * .01f);
                        }
                    }
                    break;
                case MutatorType.EnemyDamage:
                    if (mutHelper.damageOnTouch != null)
                    {
                        mutHelper.damageOnTouch.damage *= (1 + mutator.currentValue * 0.01f);
                    }
                    break;
                case MutatorType.EnemyExplode:
                    if (mutHelper.deathExplosion != null && mutHelper.enemyDeathDamageExplosion != null)
                    {
                        float explodeChance = mutator.currentValue;
                        if (Random.Range(0, 100.0f) < explodeChance)
                        {
                            mutHelper.deathExplosion.explosionObject = mutHelper.enemyDeathDamageExplosion;
                        }
                    }
                    break;
                case MutatorType.EnemyHP:
                    if (mutHelper.takesDamage != null)
                    {
                        mutHelper.takesDamage.MaxHitPoints = (int)(mutHelper.takesDamage.MaxHitPoints * (1 + mutator.currentValue * 0.01f));
                        mutHelper.takesDamage.CurrentHP = mutHelper.takesDamage.MaxHitPoints;
                    }
                    break;
                case MutatorType.EnemyRegen:
                    if (mutHelper.takesDamage != null)
                    {
                        mutHelper.takesDamage.HPRegenAmount = mutator.currentValue;                        
                    }
                    break;
                case MutatorType.EnemyInvulnerable:
                    break;
                case MutatorType.EnemySplit:
                    if (mutHelper.splitEnemyEffect != null)
                    {
                        float splitChance = mutator.currentValue;
                        if (Random.Range(0, 100.0f) < splitChance)
                        {
                            mutHelper.splitEnemyEffect.numEnemies = 2;
                        }
                    }
                    break;
                case MutatorType.SpawnTime:
                    break;
                case MutatorType.WildBullets:
                    break;
                case MutatorType.TilesetSwap:
                    break;
                case MutatorType.EnemyMumble:
                    break;
                case MutatorType.HeavyBullets:
                    break;
                case MutatorType.RandomPlayerForce:
                    break;
                default:
                    break;
            }
        }
    }

}
