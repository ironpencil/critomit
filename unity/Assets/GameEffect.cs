using UnityEngine;
using System.Collections;

public abstract class GameEffect : MonoBehaviour {

    public abstract void ActivateEffect(GameObject activator, float value);

}
