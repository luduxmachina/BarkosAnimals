using UnityEngine;

public interface IPlaceableObjectData
{
    string Name { get; }
    int ID { get; set; }
    GameObject Prefab { get; }
    Vector2Int Size { get; }
    CustomBoolMatrix OcupiedSpace { get; }
}