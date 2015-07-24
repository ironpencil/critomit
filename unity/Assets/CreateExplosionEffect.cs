using UnityEngine;
using System.Collections;

public class CreateExplosionEffect : GameEffect {

    public BaseExplosion explosionObject;
    private Transform explodeAt;

    private bool inEffect = false;

    public override void ActivateEffect(GameObject activator, float value)
    {
        if (!inEffect)
        {
            inEffect = true;

            explodeAt = activator.transform;

            DoEffect();
        }

    }

    private void DoEffect()
    {
        BaseExplosion explosion = (BaseExplosion) GameObject.Instantiate(explosionObject, explodeAt.position, explodeAt.rotation);

        explosion.transform.parent = ObjectManager.Instance.dynamicObjects.transform;

        inEffect = false;
    }

}
