using UnityEngine;

public class GridPlacementState : IGridBuildingState
{
    private int selectedObjectIndex = -1;
    private int id;
    
    private Grid grid;
    private GridPreview gridPreview;
    private ShipPlaceableObjectsSO dataBase;
    private GridData gridObjectsData;
    private IObjectPlacer objectPlacer;

    public GridPlacementState(int id, Grid grid, GridPreview gridPreview, ShipPlaceableObjectsSO dataBase,
        GridData gridObjectsData, IObjectPlacer objectPlacer)
    {
        this.id = id;
        this.grid = grid;
        this.gridPreview = gridPreview;
        this.dataBase = dataBase;
        this.gridObjectsData = gridObjectsData;
        this.objectPlacer = objectPlacer;
        
        selectedObjectIndex = dataBase.shipObjectData.FindIndex(data => data.ID == id);
        if (selectedObjectIndex > -1)
        {
            gridPreview.StartPreview(dataBase.shipObjectData[selectedObjectIndex].Prefab, dataBase.shipObjectData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No objects with this ID: {id}");
        }
    }

    public void EndState()
    {
        gridPreview.StopPreview();
    }

    public void OnAction(Vector3 position)
    {
        Vector3Int cellPos = grid.WorldToCell(position);
        cellPos.y = 0;
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);

        // Check if can be placed
        Vector2Int relativeCellPos = new Vector2Int(cellPos.x, cellPos.z);
        bool validPlace = CheckPlacementValidity(relativeCellPos, selectedObjectIndex);
        if (!validPlace)
            return;

        // Place Object
        int gameObjectIndex = objectPlacer.PlaceObject(dataBase.shipObjectData[selectedObjectIndex].Prefab, worldCellPos);
        
        GridData selectedGrid = GetSlelectedGrid(selectedObjectIndex); 
        selectedGrid.AddObject(relativeCellPos, dataBase.shipObjectData[selectedObjectIndex].OcupiedSpace, selectedObjectIndex, gameObjectIndex);
        gridPreview.UpdatePosition(worldCellPos,false, grid.cellSize.x, false);
    }

    public void UpdateState(Vector3Int cellPos)
    {
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        
        // Check if can be placed
        Vector2Int relativeCellPos = new Vector2Int(cellPos.x, cellPos.z);
        bool validPlace = CheckPlacementValidity(relativeCellPos, selectedObjectIndex);
        
        gridPreview.UpdatePosition(worldCellPos, validPlace, grid.cellSize.x, false);
    }
    
    private bool CheckPlacementValidity(Vector2Int relativeCellPos, int objectID)
    {
        GridData selectedGrid = GetSlelectedGrid(objectID); 
        return selectedGrid.CanPlaceObjectAt(relativeCellPos, dataBase.shipObjectData[selectedObjectIndex].OcupiedSpace);
    }
    
    private GridData GetSlelectedGrid(int objectID)
    {
        return gridObjectsData; // Can be selected multiple gridDatas in order to select floor or walls if needed
    }
}
