using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private List<GameObject> placedObjects = new List<GameObject>();


    public int PlaceObject(GameObject prefab, Vector3 worldCellPos, float orientation)
    {
        GameObject newObj = Instantiate(prefab, worldCellPos, Quaternion.Euler(0f, orientation, 0f));
        placedObjects.Add(newObj);
        
        return placedObjects.Count - 1;
    }

    public void RemoveObject(int gameObjectIndex)
    {
        if(placedObjects.Count <= gameObjectIndex || placedObjects[gameObjectIndex] == null)
            return;
        
        Destroy(placedObjects[gameObjectIndex]);
        placedObjects[gameObjectIndex] = null;
    }
}
