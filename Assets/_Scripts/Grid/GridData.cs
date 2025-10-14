using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector2Int, PlacementData> placedObjects = new Dictionary<Vector2Int, PlacementData>();

    public void AddObject(Vector2Int gridPos, float rotation, Vector2Int objSize, int id, int placedObjectIndex)
    {
        List<Vector2Int> positionsToOccupy = CalculatePositions(gridPos, rotation, objSize);
        PlacementData data = new PlacementData(positionsToOccupy, id, placedObjectIndex);
        
        foreach (var position in positionsToOccupy)
        {
            if(placedObjects.ContainsKey(position))
                throw new Exception($"HashSet already contains position {position}");
            
            placedObjects.Add(position, data);
        }
    }
   
    public bool CanPlaceObjectAt(Vector2Int gridPos, Vector2Int objSize)
    {
        List<Vector2Int> positionsToOccupy = CalculatePositions(gridPos, 0f, objSize);
        foreach (var position in positionsToOccupy)
        {
            if(placedObjects.ContainsKey(position))
                return false;
        }
            
        return true;
    }

    public bool HasThisPositionThisPlacementIndex(Vector2Int gridPos, int placementIDToCheck)
    {
        if (!placedObjects.TryGetValue(gridPos, out var placementData))
            return false;
        
        return placementData.placementIndex == placementIDToCheck;
    }
    
    public bool HasThisPositionThisObjectIndex(Vector2Int gridPos, int objectIDToCheck)
    {
        if (!placedObjects.TryGetValue(gridPos, out var placementData))
            return false;
        
        return placementData.id == objectIDToCheck;
    }

    public int GetRepresentationIndex(Vector2Int gridPos)
    {
        if (!placedObjects.TryGetValue(gridPos, out var placementData))
            return -1;
        
        return placementData.placementIndex;
    }

    public List<Vector2Int> GetObjectOccupiedPositions(Vector2Int cellPos)
    {
        if (!placedObjects.TryGetValue(cellPos, out var placementData))
            return null;

        return placementData.occupiedPositions;
    }

    public void RemoveObjectAt(Vector2Int cellPos)
    {
        foreach(var pos in placedObjects[cellPos].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
    
    private List<Vector2Int> CalculatePositions(Vector2Int gridPos, float rotation, Vector2Int objSize)
    {
        List<Vector2Int> returnValues = new List<Vector2Int>();
        for (int x = 0; x < objSize.x; x++)
        {
            for (int y = 0; y < objSize.y; y++)
            {
                // returnValues.Add(gridPos +  new Vector2Int(x, y));
                Vector2Int rotatedOffset = new Vector2Int(x, y);

                switch (rotation)
                {
                    case 0:
                        rotatedOffset = new Vector2Int(x, y);
                        break;

                    case -90:
                        // rotar 90° en sentido horario alrededor del (0,0)
                        rotatedOffset = new Vector2Int(y - 1, objSize.x - 1 - x);
                        break;

                    case 180:
                        rotatedOffset = new Vector2Int(objSize.x - 1 - x, objSize.y - 1 - y);
                        break;

                    case 90:
                        // rotar 270° horario (o 90° antihorario)
                        rotatedOffset = new Vector2Int(objSize.y - 1 - y, x);
                        break;

                    default:
                        Debug.LogWarning($"Rotación {rotation} no soportada. Solo 0, 90, 180, -90.");
                        break;
                }

                returnValues.Add(gridPos + rotatedOffset);
            }
        }
        
        return returnValues;
    }
}

internal class PlacementData
{
    public List<Vector2Int> occupiedPositions;
    public int id { get; private set; }
    public int placementIndex { get; private set; }

    public PlacementData(List<Vector2Int> occupiedPositions, int id, int placementIndex)
    {
        this.occupiedPositions = occupiedPositions;
        this.id = id;
        this.placementIndex = placementIndex;
    }
}
