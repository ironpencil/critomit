using UnityEngine;
using System.Collections;

public class FlashDamagedEffect : DamagedEffect {

    public SpriteRenderer spriteToFlash;
    public Color targetColor = Color.red;
    public float duration = 0.05f;

    private bool inEffect = false;

    public override void Damaged(float damage)
    {
        if (!inEffect)
        {
            inEffect = true;
            StartCoroutine(DoFlash());
        }

    }

    private IEnumerator DoFlash()
    {
        Color originalColor = spriteToFlash.color;

        spriteToFlash.color = targetColor;

        yield return new WaitForSeconds(duration);

        spriteToFlash.color = originalColor;

        inEffect = false;
    }

}
