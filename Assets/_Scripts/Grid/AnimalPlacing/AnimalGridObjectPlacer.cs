using System.Collections.Generic;
using UnityEngine;

public class AnimalGridObjectPlacer : MonoBehaviour, IObjectPlacer
{
    [SerializeField] private Transform parent;
    [SerializeField] private float heightOffset = 0.5f;
    [SerializeField] private ShipInventorySO shipInventory;
    
    private List<GameObject> placedObjects = new List<GameObject>();
    
    public int PlaceObject(GameObject prefab, Vector3 worldCellPos)
    {
        GameObject newObj;
        if (parent == null)
        {
            newObj = Instantiate(prefab);
        }
        else
        {
            newObj = Instantiate(prefab, parent);
        }
        
        worldCellPos = new Vector3(worldCellPos.x, worldCellPos.y + heightOffset, worldCellPos.z);
        
        newObj.transform.position = worldCellPos;
        placedObjects.Add(newObj);
        
        if(shipInventory != null)
            newObj.GetComponent<InformWhenRemovingFromGrid>().SetShipInventory(shipInventory);

        return placedObjects.Count - 1;
    }

    public int PlaceObject(GameObject prefab, Vector3 worldCellPos, Transform parentTransform)
    {
        GameObject newObj = Instantiate(prefab, parentTransform);
        
        worldCellPos = new Vector3(worldCellPos.x, worldCellPos.y + heightOffset, worldCellPos.z);
        
        newObj.transform.position = worldCellPos;
        placedObjects.Add(newObj);

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
