using System.Collections.Generic;

public interface IPlaceableObjectsSO<T> where T : IPlaceableObjectData
{
    List<T> PlaceableObjectData { get; }
}