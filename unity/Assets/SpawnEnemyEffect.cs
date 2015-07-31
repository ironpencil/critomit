using UnityEngine;
using System.Collections;

public class SpawnEnemyEffect : GameEffect {

    public int numEnemies = 0;
    public SpawnManager.EnemyName enemyName;

    public override void ActivateEffect(GameObject activator, float value)
    {
        for (int i = 0; i < numEnemies; i++)
        {
            SpawnManager.Instance.SpawnEnemy(enemyName, gameObject.transform);
        }
    }
}
