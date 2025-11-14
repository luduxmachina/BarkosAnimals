using System.Collections.Generic;
using UnityEngine;

public class WorldObjectPlacer : MonoBehaviour, IObjectPlacer
{
    [SerializeField]
    private LayerMask HeightCheckLayerMask;
    [SerializeField]
    private Transform parentTransform;

    private List<GameObject> placedObjects = new List<GameObject>();
    
    public int PlaceObject(GameObject prefab, Vector3 worldCellPos)
    {
        // Calcular la componente Y para que coincida con el suelo
        float y = GetHighestY(worldCellPos);
        worldCellPos = new Vector3(worldCellPos.x, y, worldCellPos.z);

        // Colocar el objeto
        GameObject newObj;
        if (parentTransform != null)
        {
            newObj = Instantiate(prefab, parentTransform);
        }
        else
        {
            newObj = Instantiate(prefab);
        }
        
        newObj.transform.position = worldCellPos;
        placedObjects.Add(newObj);

        // return gameObjectIndex
        return placedObjects.Count - 1;
    }

    public void RemoveObject(int gameObjectIndex)
    {
        throw new System.NotImplementedException();
    }

    private float GetHighestY(Vector3 position)
    {
        Vector3 start = new Vector3(position.x, 10000f, position.z);
        Vector3 direction = Vector3.down;

        RaycastHit hit;

        if (Physics.Raycast(start, direction, out hit, Mathf.Infinity, HeightCheckLayerMask))
        {
            return hit.point.y;   // Y del punto de colisi�n
        }

        return 0; // No golpe� nada
    }

}
