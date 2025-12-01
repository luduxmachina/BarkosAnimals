using System;
using UnityEngine;

public class DirtInstance : MonoBehaviour
{
    public DirtCreator dirtCreator;

    private void OnDestroy()
    {
        dirtCreator?.RemoveDirt(transform.position);
    }
}
