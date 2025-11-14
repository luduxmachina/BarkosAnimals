using UnityEngine;

public interface IObjectPlacer
{
    int PlaceObject(GameObject prefab, Vector3 worldCellPos);
    void RemoveObject(int gameObjectIndex);
}