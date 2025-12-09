using System;
using UnityEngine;

public class SetMaterialToHighRenderQueue : MonoBehaviour
{
    public Material material;
    public int renderQueue;

    private void OnEnable()
    {
        material.renderQueue = renderQueue;
    }
}
