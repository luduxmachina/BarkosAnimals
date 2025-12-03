using System;
using UnityEngine;

public class DirtInstance : MonoBehaviour
{
    public DirtCreator dirtCreator;
    public float duration = 0.3f;          // Tiempo que tarda en aparecer
    public float startScale = 0.1f;        // Tamaño inicial pequeño

    private Vector3 originalScale;

    private void OnEnable()
    {
        if (originalScale == Vector3.zero)
            originalScale = transform.localScale;

        transform.localScale = originalScale * startScale;

        StartCoroutine(ScaleIn());
    }

    private void OnDestroy()
    {
        dirtCreator?.RemoveDirt(transform.position);
    }
    
    private System.Collections.IEnumerator ScaleIn()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            transform.localScale = Vector3.Lerp(
                originalScale * startScale,
                originalScale,
                Mathf.SmoothStep(0, 1, lerp)
            );

            yield return null;
        }

        transform.localScale = originalScale;
    }
}
