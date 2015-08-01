using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndicatorPanel : MonoBehaviour {

    public EnemyIndicator enemyIndicatorPrefab;
    public EnemyIndicator spawnerIndicatorPrefab;

    public List<EnemyIndicator> enemyIndicators = new List<EnemyIndicator>();

    public Vector2 indicatorBoundsX = new Vector2(-400.0f, 400.0f);
    public Vector2 indicatorBoundsY = new Vector2(-300.0f, 300.0f);

    //public Vector2 visibleBoundsX = new Vector2(-8.0f, 8.0f);
    //public Vector2 visibleBoundsY = new Vector2(-6.0f, 6.0f);

    public RectTransform enemyParentRect;
    public RectTransform spawnerParentRect;

    public RectTransform mainCanvas;

    public Transform testTransform;

    public enum IndicatorType
    {
        Enemy,
        Spawner
    }

	// Use this for initialization
	void Start () {

        //rect = gameObject.GetComponent<RectTransform>();
	
	}
	
	// Update is called once per frame
	void Update () {

        //indicatorCenter = transform.position; //is this going to give us world center or screen center?

        List<EnemyIndicator> removeIndicators = new List<EnemyIndicator>();

        foreach (EnemyIndicator indicator in enemyIndicators)
        {
            if (UpdateIndicator(indicator))
            {
                removeIndicators.Add(indicator);
            }
        }

        foreach (EnemyIndicator removedIndicator in removeIndicators)
        {
            //enemyIndicators.Remove(removedIndicator);
            RemoveIndicator(removedIndicator);
        }
	}

    [ContextMenu("Add Test Indicator")]
    public void AddTestIndicator()
    {
        AddIndicator(testTransform, IndicatorType.Enemy);
    }

    public void AddIndicator(Transform enemy, IndicatorType indicatorType)
    {
        EnemyIndicator indicatorPrefab;
        RectTransform parentRect;

        switch (indicatorType)
        {
            case IndicatorType.Enemy:
                indicatorPrefab = enemyIndicatorPrefab;
                parentRect = enemyParentRect;
                break;
            case IndicatorType.Spawner:
                indicatorPrefab = spawnerIndicatorPrefab;
                parentRect = spawnerParentRect;
                break;
            default:
                return;                
        }

        EnemyIndicator newIndicator = (EnemyIndicator)GameObject.Instantiate(indicatorPrefab, transform.position, transform.rotation);

        newIndicator.indicatorRect.SetParent(parentRect, false);

        newIndicator.enemy = enemy;

        enemyIndicators.Add(newIndicator);

        newIndicator.isVisible = true;
    }

    private bool UpdateIndicator(EnemyIndicator indicator)
    {
        if (indicator.enemy == null)
        {
            return true; //true = remove this indicator
        }

        //not destroyed

        

        //indicator is only visible when enemy is not
        bool indicatorVisible = !TransformIsVisible(indicator.enemy);


        if (indicatorVisible)
        {
            PositionIndicator(indicator);
        }

        if (indicatorVisible != indicator.isVisible)
        {
            indicator.isVisible = indicatorVisible;
            indicator.gameObject.SetActive(indicatorVisible);
        }

        return false;
    }

    private void RemoveIndicator(EnemyIndicator indicator)
    {
        // enemy has been destroyed, destroy indicator
        enemyIndicators.Remove(indicator);
        indicator.gameObject.SetActive(false);
        Destroy(indicator.gameObject);
    }

    private void PositionIndicator(EnemyIndicator indicator)
    {
        Vector2 viewPos = Camera.main.WorldToViewportPoint(indicator.enemy.position);

        Vector2 enemyPos = Vector2.zero;

        enemyPos.x = (viewPos.x * mainCanvas.sizeDelta.x) - (mainCanvas.sizeDelta.x * 0.5f);
        enemyPos.y = (viewPos.y * mainCanvas.sizeDelta.y) - (mainCanvas.sizeDelta.y * 0.5f);        

        //Vector2 indicatorPos = Vector2.zero;

        //if (Mathf.Abs(enemyPos.x) > Mathf.Abs(enemyPos.y))
        //{
        //    //calculate y where x = 1
        //    indicatorPos.x = enemyPos.x / Mathf.Abs(enemyPos.x);
        //    indicatorPos.y = enemyPos.y / Mathf.Abs(enemyPos.x);

        //}
        //else
        //{
        //    indicatorPos.y = enemyPos.y / Mathf.Abs(enemyPos.y);
        //    indicatorPos.x = enemyPos.x / Mathf.Abs(enemyPos.y);
        //}

        ////assume width > height

        //indicatorPos.x *= indicatorBoundsX.y;
        //indicatorPos.y *= indicatorBoundsX.y;        

        //if (Mathf.Abs(indicatorPos.y) > indicatorBoundsY.y)
        //{
        //    //out of bounds
        //    float boundsExceededRatio = indicatorBoundsY.y / Mathf.Abs(indicatorPos.y);

        //    indicatorPos.y *= boundsExceededRatio;
        //    indicatorPos.x *= boundsExceededRatio;

        //}





        Vector2 enemyNormalized = enemyPos.normalized;

        Vector2 indicatorPos = enemyNormalized;

        //gets circular notifier
        if (indicatorPos.x < 0.0f)
        {
            indicatorPos.x *= indicatorBoundsX.x * -1;
        }
        else
        {
            indicatorPos.x *= indicatorBoundsX.y;
        }

        if (indicatorPos.y < 0.0f)
        {
            indicatorPos.y *= indicatorBoundsY.x * -1;
        }
        else
        {
            indicatorPos.y *= indicatorBoundsY.y;
        }


        //find out which magnitude is bigger
        //if (Mathf.Abs(indicatorPos.x) > Mathf.Abs(indicatorPos.y))
        //{
        //    //x is bigger, so arrow is on left/right side
        //    indicatorPos *= indicatorBoundsX.y;
        //}
        //else
        //{
        //    //y is bigger, so arrow is on top/bottom side
        //    indicatorPos *= indicatorBoundsY.y;
        //}


        //y = ((y2-y1)/(x2-x1))x + c
        
        //assuming enemyPos is positive on both axis
        //x1,y1 = 0,0
        //x2,y2 = enemyPos.x,enemyPos.y
        //x = indicatorBoundsX.y (max x boundary)
        //c = y intercept (0)

        //indicatorPos.y = ((enemyPos.y - 0)/(enemyPos.x - 0)) * indicatorBoundsX.y + 0

        
        
        
        //Vector2 indicatorPos = enemyPos;

        //indicatorPos.x = Mathf.Clamp(enemyPos.x, indicatorBoundsX.x, indicatorBoundsX.y);
        //indicatorPos.y = Mathf.Clamp(enemyPos.y, indicatorBoundsY.x, indicatorBoundsY.y);

        indicator.indicatorRect.anchoredPosition = indicatorPos;

        Vector3 thisPos = Vector2.zero;

        Vector2 offset = new Vector2(enemyPos.x - thisPos.x, enemyPos.y - thisPos.y);

        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        indicator.indicatorRect.rotation = Quaternion.Euler(0, 0, angle);



        //Vector2 viewPos = Camera.main.WorldToViewportPoint(indicator.enemy.position);

        //Vector2 enemyPos = Vector2.zero;

        //enemyPos.x = (viewPos.x * mainCanvas.sizeDelta.x) - (mainCanvas.sizeDelta.x * 0.5f);
        //enemyPos.y = (viewPos.y * mainCanvas.sizeDelta.y) - (mainCanvas.sizeDelta.y * 0.5f);

        //Vector2 indicatorPos = enemyPos;

        //indicatorPos.x = Mathf.Clamp(enemyPos.x, indicatorBoundsX.x, indicatorBoundsX.y);
        //indicatorPos.y = Mathf.Clamp(enemyPos.y, indicatorBoundsY.x, indicatorBoundsY.y);

        //indicator.indicatorRect.anchoredPosition = indicatorPos;

        //Vector3 thisPos = Vector2.zero;

        //Vector2 offset = new Vector2(enemyPos.x - thisPos.x, enemyPos.y - thisPos.y);

        //var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        //indicator.indicatorRect.rotation = Quaternion.Euler(0, 0, angle);


        
    }

    private bool TransformIsVisible(Transform target)
    {

        Vector2 viewPos = Camera.main.WorldToViewportPoint(target.position);

        bool visible = viewPos.x > 0.0f && viewPos.x < 1.0f && viewPos.y > 0.0f && viewPos.y < 1.0f;

        return visible;

        //return RectTransformUtility.RectangleContainsScreenPoint(rect, target.position, null);
    }
}
