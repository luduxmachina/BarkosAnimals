using UnityEngine;

public class GridPlacementState : IGridBuildingState
{
    private int selectedObjectIndex = -1;
    private int id;
    
    private Grid grid;
    private GridPreview gridPreview;
    private ABasePlaceableObjectsSO dataBase;
    private GridData gridObjectsData;
    private IObjectPlacer objectPlacer;

    public GridPlacementState(int id, Grid grid, GridPreview gridPreview, ABasePlaceableObjectsSO dataBase,
        GridData gridObjectsData, IObjectPlacer objectPlacer)
    {
        this.id = id;
        this.grid = grid;
        this.gridPreview = gridPreview;
        this.dataBase = dataBase;
        this.gridObjectsData = gridObjectsData;
        this.objectPlacer = objectPlacer;
        
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
            throw new System.Exception($"No objects with this ID: {id}");
        }
    }

    public void EndState()
    {
        gridPreview?.StopPreview();
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
        var placeableObjects = dataBase.GetPlaceableObjects();
        
        int gameObjectIndex = objectPlacer.PlaceObject(placeableObjects[selectedObjectIndex].Prefab, worldCellPos);
        
        GridData selectedGrid = GetSlelectedGrid(selectedObjectIndex); 
        selectedGrid.AddObject(relativeCellPos, placeableObjects[selectedObjectIndex].OcupiedSpace, selectedObjectIndex, gameObjectIndex);
        gridPreview?.UpdatePosition(worldCellPos,false, grid.cellSize.x, false);
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
        
        GridData selectedGrid = GetSlelectedGrid(objectID); 
        return selectedGrid.CanPlaceObjectAt(relativeCellPos, placeableObjects[selectedObjectIndex].OcupiedSpace);
    }
    
    private GridData GetSlelectedGrid(int objectID)
    {
        return gridObjectsData; // Can be selected multiple gridDatas in order to select floor or walls if needed
    }
}
