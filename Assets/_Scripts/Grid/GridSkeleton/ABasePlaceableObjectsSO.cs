using System.Collections.Generic;
using UnityEngine;

public abstract class ABasePlaceableObjectsSO : ScriptableObject
{
    public abstract IReadOnlyList<IPlaceableObjectData> GetPlaceableObjects();
    protected PlaceableDatabaseType placeableType;
    public PlaceableDatabaseType PlaceableType => placeableType;
}

public enum PlaceableDatabaseType
{
    WorldB,
    WorldC,
    ShipBuildable,
}
