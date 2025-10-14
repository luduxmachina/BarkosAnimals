using System.Collections.Generic;
using UnityEngine;

public class GridRemovingState : IGridBuildingState
{
    private int gameObjectIndex = -1;
    
    private Grid grid;
    private GridPreview gridPreview;
    private GridData gridObjectsData;
    private ObjectPlacer objectPlacer;
    
    private int pastPlacementID = -1;

    public GridRemovingState(Grid grid, GridPreview gridPreview, GridData gridObjectsData, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.gridPreview = gridPreview;
        this.gridObjectsData = gridObjectsData;
        this.objectPlacer = objectPlacer;
        
        gridPreview.StartRemovePreview();
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
                return;

            selectedData.RemoveObjectAt(relativeCellPos);
            objectPlacer.RemoveObject(gameObjectIndex);
        }
        
        gridPreview.UpdatePosition(worldCellPos, orientation, ChechIfSelectionIsValid(relativeCellPos), grid.cellSize.x, true);
        gridPreview.EraseRemovePreview();
    }

    public void UpdateState(Vector3Int cellPos, float orientation)
    {
        cellPos.y = 0;
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        Vector2Int relativeCellPos = new Vector2Int(cellPos.x, cellPos.z);
        
        bool validity = ChechIfSelectionIsValid(relativeCellPos);
        int placementID = GetPlacementID(relativeCellPos);
        gridPreview.UpdatePosition(worldCellPos, orientation, validity, grid.cellSize.x, true);
        
        
        if (placementID != pastPlacementID)
        {
            // We erase the remove preview if we select another object
            gridPreview.EraseRemovePreview();

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

                gridPreview.UpdateRemovePreview(occupiedWorldPositions);
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
        return gridObjectsData; // Can be selected multiple gridDatas in order to select floor or walls if needed
    }
}
