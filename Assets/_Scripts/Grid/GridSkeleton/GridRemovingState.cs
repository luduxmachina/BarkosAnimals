using System.Collections.Generic;
using UnityEngine;

public class GridRemovingState : IGridBuildingState
{
    private int gameObjectIndex = -1;
    
    private Grid grid;
    private GridPreview gridPreview;
    private List<GridData> gridObjectsDatas;
    private IObjectPlacer objectPlacer;
    
    private int pastPlacementID = -1;

    public GridRemovingState(Grid grid, GridPreview gridPreview, List<GridData> gridObjectsDatas, IObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.gridPreview = gridPreview;
        this.gridObjectsDatas = gridObjectsDatas;
        this.objectPlacer = objectPlacer;
        
        gridPreview?.StartRemovePreview();
    }

    public void EndState()
    {
        gridPreview?.StopPreview();
    }

    public bool OnAction(Vector3 position)
    {
        Vector3Int cellPos = grid.WorldToCell(position);
        cellPos.y = 0;
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        
        Vector2Int relativeCellPos = new Vector2Int(cellPos.x, cellPos.z);

        GridData selectedData = GetSlelectedGrid();

        if (selectedData == null)
        {
            // Call event for faild attempt to remove
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(relativeCellPos);
            if(gameObjectIndex < 0)
                return false;

            selectedData.RemoveObjectAt(relativeCellPos);
            objectPlacer.RemoveObject(gameObjectIndex);
        }
        
        gridPreview?.UpdatePosition(worldCellPos, ChechIfSelectionIsValid(relativeCellPos), grid.cellSize.x, true);
        gridPreview?.EraseRemovePreview();
        return true;
    }

    public void UpdateState(Vector3Int cellPos)
    {
        cellPos.y = 0;
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        Vector2Int relativeCellPos = new Vector2Int(cellPos.x, cellPos.z);
        
        bool validity = ChechIfSelectionIsValid(relativeCellPos);
        int placementID = GetPlacementID(relativeCellPos);
        gridPreview?.UpdatePosition(worldCellPos, validity, grid.cellSize.x, true);
        
        if (placementID != pastPlacementID)
        {
            // We erase the remove preview if we select another object
            gridPreview?.EraseRemovePreview();

            // We show the actual remove preview
            if (validity)
            {
                GridData selectedGrid = GetSlelectedGrid();
                List<Vector2Int> occupiedLocalPositions = selectedGrid.GetObjectOccupiedPositions(relativeCellPos);
                List<Vector3> occupiedWorldPositions = new List<Vector3>();

                for (int i = 0; i < occupiedLocalPositions.Count; i++)
                {
                    Vector3Int aux;
                    aux = new Vector3Int(occupiedLocalPositions[i].x, 0, occupiedLocalPositions[i].y);
                    occupiedWorldPositions.Add(grid.GetCellCenterWorld(aux));
                }

                gridPreview?.UpdateRemovePreview(occupiedWorldPositions);
            }
        }
        
        pastPlacementID = placementID;
    }
    
    private bool ChechIfSelectionIsValid(Vector2Int relativeCellPos)
    {
        GridData selectedGrid = GetSlelectedGrid(); 
        return !(selectedGrid.CanPlaceObjectAt(relativeCellPos, Vector2Int.one));
    }

    private int GetPlacementID(Vector2Int relativeCellPos)
    {
        GridData selectedGrid = GetSlelectedGrid();
        return selectedGrid.GetRepresentationIndex(relativeCellPos);
    }

    private GridData GetSlelectedGrid()
    {
        // if (dataBase.PlaceableType == PlaceableDatabaseType.ShipBuildable)
        //     return gridObjectsDatas[0];
        // 
        // if (dataBase.PlaceableType == PlaceableDatabaseType.WorldB)
        //     return gridObjectsDatas[0];
        // 
        // if (dataBase.PlaceableType == PlaceableDatabaseType.WorldC)
        //     return gridObjectsDatas[1];

        // Default
        return gridObjectsDatas[0];
    }
}
