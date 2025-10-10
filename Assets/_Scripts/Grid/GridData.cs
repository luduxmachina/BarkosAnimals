using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    HashSet<Vector2> ocupiedPositions = new HashSet<Vector2>();

    public void AddObject(Vector2Int gridPos, Vector2Int objSize)
    {
        List<Vector2Int> positionsToOccupy = CalculatePositions(gridPos, objSize);
        foreach (var position in positionsToOccupy)
        {
            if(ocupiedPositions.Contains(position))
                throw new Exception($"HashSet already contains position {position}");
            
            ocupiedPositions.Add(position);
        }
    }

    private List<Vector2Int> CalculatePositions(Vector2Int gridPos, Vector2Int objSize)
    {
        List<Vector2Int> returnValues = new List<Vector2Int>();
        for (int x = 0; x < objSize.x; x++)
        {
            for (int y = 0; y < objSize.y; y++)
            {
                returnValues.Add(gridPos +  new Vector2Int(x, y));
            }
        }
        
        return returnValues;
    }

    public bool CanPlaceObjectAt(Vector2Int gridPos, Vector2Int objSize)
    {
        List<Vector2Int> positionsToOccupy = CalculatePositions(gridPos, objSize);
        foreach (var position in positionsToOccupy)
        {
            if(ocupiedPositions.Contains(position))
                return false;
        }
            
        return true;
    }
}
