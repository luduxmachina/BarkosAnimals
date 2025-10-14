using UnityEngine;

public class GridPlacementState : IGridBuildingState
{
    private int selectedObjectIndex = -1;
    private int id;
    
    private Grid grid;
    private GridPreview gridPreview;
    private PlaceableObjectsSO dataBase;
    private GridData gridObjectsData;
    private ObjectPlacer objectPlacer;

    public GridPlacementState(int id, Grid grid, GridPreview gridPreview, PlaceableObjectsSO dataBase,
        GridData gridObjectsData, ObjectPlacer objectPlacer)
    {
        this.id = id;
        this.grid = grid;
        this.gridPreview = gridPreview;
        this.dataBase = dataBase;
        this.gridObjectsData = gridObjectsData;
        this.objectPlacer = objectPlacer;
        
        selectedObjectIndex = dataBase.objectData.FindIndex(data => data.ID == id);
        if (selectedObjectIndex > -1)
        {
            gridPreview.StartPreview(dataBase.objectData[selectedObjectIndex].Prefab, dataBase.objectData[selectedObjectIndex].Size, 0f);
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

    public void OnAction(Vector3 position, float orientation)
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
        int gameObjectIndex = objectPlacer.PlaceObject(dataBase.objectData[selectedObjectIndex].Prefab, worldCellPos, orientation);
        
        GridData selectedGrid = GetSlelectedGrid(selectedObjectIndex); 
        selectedGrid.AddObject(relativeCellPos, orientation, dataBase.objectData[selectedObjectIndex].Size, selectedObjectIndex, gameObjectIndex);
        gridPreview.UpdatePosition(worldCellPos, orientation,false, grid.cellSize.x, false);
    }

    public void UpdateState(Vector3Int cellPos, float orientation)
    {
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        
        // Check if can be placed
        Vector2Int relativeCellPos = new Vector2Int(cellPos.x, cellPos.z);
        bool validPlace = CheckPlacementValidity(relativeCellPos, selectedObjectIndex);
        
        gridPreview.UpdatePosition(worldCellPos, orientation, validPlace, grid.cellSize.x, false);
    }
    
    private bool CheckPlacementValidity(Vector2Int relativeCellPos, int objectID)
    {
        GridData selectedGrid = GetSlelectedGrid(objectID); 
        return selectedGrid.CanPlaceObjectAt(relativeCellPos, dataBase.objectData[selectedObjectIndex].Size);
    }
    
    private GridData GetSlelectedGrid(int objectID)
    {
        return gridObjectsData; // Can be selected multiple gridDatas in order to select floor or walls if needed
    }
}
