using UnityEngine;
using System.Collections;

public class EnemyIndicator : MonoBehaviour {

    public RectTransform indicatorRect;
    public Transform enemy;
    public bool isVisible;
	

	void Awake () {
        if (indicatorRect == null)
        {
            indicatorRect = gameObject.GetComponent<RectTransform>();
        }
	}
	
	// Update is called once per frame
	void Update () {

        //Vector2 worldPos = RectTransformUtility.WorldToScreenPoint(null, indicatorRect.position);
        //Debug.Log("Pos=" + indicatorRect.position + "|APos=" + indicatorRect.anchoredPosition + "|WPos=" + worldPos);      
	}
}
