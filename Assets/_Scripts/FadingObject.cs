using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadingObject : MonoBehaviour
{
    float alpha = 1f;
    Material material;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }
    public void FadeIn(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(_FadeIn(duration));
    }
    public void FadeOut(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(_FadeOut(duration));
    }

    private IEnumerator _FadeIn(float duration)
    {
        while (alpha < 1)
        {
            alpha = Mathf.Clamp((alpha + Time.deltaTime / duration), 0f, 1f);

            Color color = material.color;
            color.a = alpha;
            material.color = color;
            yield return null;
        }
    }
    private IEnumerator _FadeOut(float duration)
    {
        while (alpha > 0)
        {
            alpha = Mathf.Clamp((alpha - Time.deltaTime / duration), 0f, 1f);

            Color color = material.color;
            color.a = alpha;
            material.color = color;
            yield return null;
        }
    }
}
