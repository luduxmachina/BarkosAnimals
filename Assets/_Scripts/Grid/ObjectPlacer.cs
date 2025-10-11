using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private HashSet<GameObject> placedObjects = new HashSet<GameObject>();


    public void PlaceObject(GameObject prefab, Vector3 worldCellPos)
    {
        GameObject newObj = Instantiate(prefab);
        newObj.transform.position = worldCellPos;
        placedObjects.Add(newObj);
    }
}
