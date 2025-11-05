using System.Collections.Generic;
using UnityEngine;

public class BasePlaceableObjectsSO<T>: ABasePlaceableObjectsSO where T : IPlaceableObjectData
{
    [field: SerializeField] public List<T> PlaceableObjectData { get; private set; } = new List<T>();
    
    public override IReadOnlyList<IPlaceableObjectData> GetPlaceableObjects() => (IReadOnlyList<IPlaceableObjectData>)PlaceableObjectData;
}