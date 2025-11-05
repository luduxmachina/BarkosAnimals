using System.Collections.Generic;
using UnityEngine;

public class WorldObjectPlacer : MonoBehaviour, IObjectPlacer
{
    private List<GameObject> placedObjects = new List<GameObject>();
    
    public int PlaceObject(GameObject prefab, Vector3 worldCellPos)
    {
        Debug.Log("Placing object");
        
        GameObject newObj = Instantiate(prefab);
        newObj.transform.position = worldCellPos;
        placedObjects.Add(newObj);

        // return gameObjectIndex
        return placedObjects.Count - 1;
    }

    public void RemoveObject(int gameObjectIndex)
    {
        throw new System.NotImplementedException();
    }
}
