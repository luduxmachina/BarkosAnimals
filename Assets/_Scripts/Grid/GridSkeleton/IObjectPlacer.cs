using UnityEngine;

public interface IObjectPlacer
{
    int PlaceObject(GameObject prefab, Vector3 worldCellPos);
    int PlaceObject(GameObject prefab, Vector3 worldCellPos, Transform parentTransform);
    void RemoveObject(int gameObjectIndex);
}