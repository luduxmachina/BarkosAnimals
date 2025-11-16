using UnityEngine;

public interface IObjectPlacer
{
    int PlaceObject(GameObject prefab, Vector3 worldCellPos);
    int PlaceObject(GameObject prefab, Vector3 worldCellPos, Transform parent);
    void RemoveObject(int gameObjectIndex);
}