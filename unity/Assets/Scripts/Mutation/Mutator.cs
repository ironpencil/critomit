using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mutator : MonoBehaviour
{
    public MutatorType mutatorType;
    public float currentValue;

    public bool isCurrentlyActive = false;

    public bool limitedApplication = true;
    public bool allowedToApply = true;

    public float initialValue;
    public float maxValue;
    public float incrementValue;

    public float initialWeight;
    public float decrementedWeight;
    public float currentWeight;
    public float cumulativeWeight;

}
