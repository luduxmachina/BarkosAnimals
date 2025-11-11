using System.Collections.Generic;
using UnityEngine;

public abstract class ABasePlaceableObjectsSO : ScriptableObject
{
    public abstract IReadOnlyList<IPlaceableObjectData> GetPlaceableObjects();
}
