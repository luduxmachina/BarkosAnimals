using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectPlacer : MonoBehaviour, IObjectPlacer
{
    [SerializeField]
    private LayerMask HeightCheckLayerMask;
    [SerializeField]
    private Transform parentTransform;

    [SerializeField] private int placingChecks = 4;

    private List<GameObject> placedObjects = new List<GameObject>();
    private List<Vector3> positions = new List<Vector3>();
    
    public int PlaceObject(GameObject prefab, Vector3 worldCellPos)
    {
        // Calcular la componente Y para que coincida con el suelo
        float y = GetHighestY(worldCellPos);
        worldCellPos = new Vector3(worldCellPos.x, y, worldCellPos.z);
        positions.Add(worldCellPos);
        
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

    private void OnDrawGizmos()
    {
        if(!Application.isPlaying)
            return;

        Gizmos.color = Color.red;
        foreach (var pos in positions)
        {
            Gizmos.DrawSphere(pos, 0.1f);
        }
    }

    public int PlaceObject(GameObject prefab, Vector3 worldCellPos, Transform parent)
    {
        // Calcular la componente Y para que coincida con el suelo
        float y = GetHighestY(worldCellPos);
        worldCellPos = new Vector3(worldCellPos.x, y, worldCellPos.z);

        // Colocar el objeto
        GameObject newObj = Instantiate(prefab, parent);
        
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
        float highestY = float.MaxValue;
        Vector3 start = new Vector3(position.x, 10000f, position.z);
        Vector3 direction = Vector3.down;

        RaycastHit hit;

        for (int i = 0; i < placingChecks; i++)
        {
            Vector3 check = new Vector3(position.x + i*0.5f, 10000f, position.z+i*0.5f);
            
            if (Physics.Raycast(check, direction, out hit, Mathf.Infinity, HeightCheckLayerMask))
            {
                highestY = Mathf.Min(hit.point.y, highestY);
            }
        }
        

        return highestY;
    }

}
