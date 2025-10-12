using System.Collections.Generic;
using UnityEngine;

public class GridRemovingState : IGridBuildingState
{
    private int gameObjectIndex = -1;
    
    private Grid grid;
    private GridPreview gridPreview;
    private GridData gridObjectsData;
    private ObjectPlacer objectPlacer;
    
    private bool pastValidity = false;

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

    public void OnAction(Vector3 position)
    {
        Vector3Int cellPos = grid.WorldToCell(position);
        cellPos.y = 0;
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        
        Vector2Int relativeCellPos = new Vector2Int(cellPos.x, cellPos.z);
        
        GridData selectedData = null;
        // if we cant place something here, there is something to be removed
        if (!gridObjectsData.CanPlaceObjectAt(relativeCellPos, Vector2Int.one))
        {
            selectedData = gridObjectsData;
        }
        else
        {
            // If we do more than one grid, make here the logic of selecting for removal
        }

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
        
        gridPreview.UpdatePosition(worldCellPos, ChechIfSelectionIsValid(relativeCellPos), grid.cellSize.x, true);
        gridPreview.ErraseRemovePreview();
    }

    public void UpdateState(Vector3Int cellPos)
    {
        cellPos.y = 0;
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        Vector2Int relativeCellPos = new Vector2Int(cellPos.x, cellPos.z);
        
        bool validity = ChechIfSelectionIsValid(relativeCellPos);
        gridPreview.UpdatePosition(worldCellPos, validity, grid.cellSize.x, true);

        if (pastValidity != validity)
        {
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
            else
            {
                gridPreview.ErraseRemovePreview();
            }  
        }
        
        pastValidity =  validity;
    }
    
    private bool ChechIfSelectionIsValid(Vector2Int relativeCellPos)
    {
        GridData selectedGrid = GetSlelectedGrid(); 
        return !(selectedGrid.CanPlaceObjectAt(relativeCellPos, Vector2Int.one));
    }
    
    private GridData GetSlelectedGrid()
    {
        return gridObjectsData; // Can be selected multiple gridDatas in order to select floor or walls if needed
    }
}
