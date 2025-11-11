using System;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacementState : IGridBuildingState
{
    private int selectedObjectIndex = -1;
    private int id;
    
    private Grid grid;
    private GridPreview gridPreview;
    private ABasePlaceableObjectsSO dataBase;
    private List<GridData> gridObjectsDatas;
    private IObjectPlacer objectPlacer;
    private Transform parentTransform;

    public GridPlacementState(int id, Grid grid, GridPreview gridPreview, ABasePlaceableObjectsSO dataBase,
        List<GridData> gridObjectsDatas, IObjectPlacer objectPlacer, Transform parentTransform)
    {
        this.id = id;
        this.grid = grid;
        this.gridPreview = gridPreview;
        this.dataBase = dataBase;
        this.gridObjectsDatas = gridObjectsDatas;
        this.objectPlacer = objectPlacer;
        this.parentTransform = parentTransform;


        var placeableObjects = dataBase.GetPlaceableObjects();
        selectedObjectIndex = -1;
        for (int i = 0; i < placeableObjects.Count; i++)
        {
            if (placeableObjects[i].ID == id)
            {
                selectedObjectIndex = i;
                break;
            }
        }
        
        if (selectedObjectIndex > -1)
        {
            gridPreview?.StartPreview(placeableObjects[selectedObjectIndex].Prefab, placeableObjects[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"[GridPlacementSatate] No objects with this ID: {id}");
        }
    }

    public void EndState()
    {
        gridPreview?.StopPreview();
    }

    public void OnAction(Vector3 position)
    {
        float girdSize = grid.cellSize.x;

        Vector3Int cellPos = grid.WorldToCell(position);
        cellPos.y = 0;
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        worldCellPos = new Vector3(worldCellPos.x + girdSize / 2, worldCellPos.y, worldCellPos.z + girdSize / 2);

        // Check if can be placed
        Vector2Int relativeCellPos = new Vector2Int(cellPos.x, cellPos.z);
        bool validPlace = CheckPlacementValidity(relativeCellPos, selectedObjectIndex);
        if (!validPlace)
            return;

        // Place Object
        var placeableObjects = dataBase.GetPlaceableObjects();

        int gameObjectIndex;
        if (parentTransform != null)
        {
            gameObjectIndex = objectPlacer.PlaceObject(placeableObjects[selectedObjectIndex].Prefab, worldCellPos, parentTransform);
        }
        else
        {
            gameObjectIndex = objectPlacer.PlaceObject(placeableObjects[selectedObjectIndex].Prefab, worldCellPos);
        }

        AddOcupiedSpaces(relativeCellPos, placeableObjects[selectedObjectIndex], selectedObjectIndex, gameObjectIndex);
        gridPreview?.UpdatePosition(worldCellPos, false, grid.cellSize.x, false);
    }

    private void AddOcupiedSpaces(Vector2Int relativeCellPos, IPlaceableObjectData ObjData, int id, int placedObjIndex)
    {
        GridData selectedGrid = GetSlelectedGrid();
        selectedGrid.AddObject(relativeCellPos, ObjData.OcupiedSpace, id, placedObjIndex);

        if(dataBase.PlaceableType == PlaceableDatabaseType.WorldB)
        {
            selectedGrid = gridObjectsDatas[1];

            B_WolrdObjectData BObjData;
            BObjData = ObjData as B_WolrdObjectData;

            selectedGrid.AddObject(relativeCellPos, BObjData.C_OcupiedSpace, id, placedObjIndex);
        }
    }

    public void UpdateState(Vector3Int cellPos)
    {
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        
        // Check if can be placed
        Vector2Int relativeCellPos = new Vector2Int(cellPos.x, cellPos.z);
        bool validPlace = CheckPlacementValidity(relativeCellPos, selectedObjectIndex);
        
        gridPreview?.UpdatePosition(worldCellPos, validPlace, grid.cellSize.x, false);
    }
    
    private bool CheckPlacementValidity(Vector2Int relativeCellPos, int objectID)
    {
        var placeableObjects = dataBase.GetPlaceableObjects();
        
        GridData selectedGrid = GetSlelectedGrid(); 
        return selectedGrid.CanPlaceObjectAt(relativeCellPos, placeableObjects[selectedObjectIndex].OcupiedSpace);
    }
    
    private GridData GetSlelectedGrid()
    {
        if(dataBase.PlaceableType == PlaceableDatabaseType.ShipBuildable)
            return gridObjectsDatas[0];

        if (dataBase.PlaceableType == PlaceableDatabaseType.WorldB)
            return gridObjectsDatas[0];

        if (dataBase.PlaceableType == PlaceableDatabaseType.WorldC)
            return gridObjectsDatas[1];

        // Default
        return gridObjectsDatas[0];
    }
}
