using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class LevelSwitcher : MonoBehaviour {

    public GameLevel levelToLoad;

    public bool activated = true;

    private bool doReactivate = false;
    private float reactivateDelay = 1.0f;
    private float reactivateTime = 0.0f;

    private bool doLoadLevel = false;

    [SerializeField]
    private UnityEvent onLoadLevel = new UnityEvent();

	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        if (!activated && doReactivate && Time.time > reactivateTime)
        {
            activated = true;
            doReactivate = false;
        }

        if (doLoadLevel)
        {
            activated = false;
            reactivateTime = Time.time + reactivateDelay;
            doReactivate = true;
            doLoadLevel = false;

            onLoadLevel.Invoke();
            Globals.Instance.LoadLevel(levelToLoad);    
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                doLoadLevel = true;
            }
        }
    }
}
