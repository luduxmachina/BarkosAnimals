using UnityEngine;

public class GridRemovingState : IGridBuildingState
{
    private int gameObjectIndex = -1;
    
    private Grid grid;
    private GridPreview gridPreview;
    private GridData gridObjectsData;
    private ObjectPlacer objectPlacer;

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
        
        Vector2Int realCellPos = new Vector2Int(cellPos.x, cellPos.z);
        
        GridData selectedData = null;
        // if we cant place something here, there is something to be removed
        if (!gridObjectsData.CanPlaceObjectAt(realCellPos, Vector2Int.one))
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
            gameObjectIndex = selectedData.GetRepresentationIndex(realCellPos);
            if(gameObjectIndex < 0)
                return;

            selectedData.RemoveObjectAt(realCellPos);
            objectPlacer.RemoveObject(gameObjectIndex);
        }
        
        gridPreview.UpdatePosition(worldCellPos,  ChechIfSelectionIsValid(cellPos), grid.cellSize.x, true);
    }

    public void UpdateState(Vector3Int cellPos)
    {
        cellPos.y = 0;
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        
        bool validity = ChechIfSelectionIsValid(cellPos);
        gridPreview.UpdatePosition(worldCellPos,  validity, grid.cellSize.x, true);
    }
    
    private bool ChechIfSelectionIsValid(Vector3Int cellPos)
    {
        Vector2Int realCellPos = new Vector2Int(cellPos.x, cellPos.z);
        return !(gridObjectsData.CanPlaceObjectAt(realCellPos, Vector2Int.one)); // If more grids are added, it needs to be updated here to
    }
}
