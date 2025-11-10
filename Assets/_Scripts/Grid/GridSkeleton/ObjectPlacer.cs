using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour, IObjectPlacer
{
    private List<GameObject> placedObjects = new List<GameObject>();

    public int PlaceObject(GameObject prefab, Vector3 worldCellPos)
    {
        GameObject newObj = Instantiate(prefab);
        newObj.transform.position = worldCellPos;
        placedObjects.Add(newObj);

        // return gameObjectIndex
        return placedObjects.Count - 1;
    }

    public void RemoveObject(int gameObjectIndex)
    {
        if (placedObjects.Count <= gameObjectIndex || placedObjects[gameObjectIndex] == null)
            return;

        Destroy(placedObjects[gameObjectIndex]);
        placedObjects[gameObjectIndex] = null;
    }
}
