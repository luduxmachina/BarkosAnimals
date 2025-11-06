using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Grid))]
[RequireComponent(typeof(IGridInput))]
[RequireComponent(typeof(IObjectPlacer))]

// [RequireComponent(typeof(GridPreview))]

public class GridPlacementManager : MonoBehaviour
{
    [SerializeField] private ABasePlaceableObjectsSO dataBase;
    // private BasePlaceableObjectsSO<PlaceableObjectDataBase> dataBase => DataBase as BasePlaceableObjectsSO<PlaceableObjectDataBase>;

    [SerializeField] private GameObject gridVisualization;

    private Grid grid;
    private IGridInput gridInput;
    private GridData gridObjectsData;
    private GridPreview gridPreview;
    private IObjectPlacer objectPlacer;
    
    private Vector3Int lastPos = Vector3Int.zero;
    
    private IGridBuildingState buildingState;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        gridInput = GetComponent<IGridInput>();
        objectPlacer = GetComponent<IObjectPlacer>();
        gridObjectsData = new GridData();

        if (TryGetComponent(out GridPreview aux))
        {
            gridPreview = aux;
        }
    }

    private void Start()
    {
        StopPlacement();
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        
        // Visual Indicators
        Vector3 selectedPos = gridInput.GetSelectedMapPosition();

        Vector3Int cellPos = grid.WorldToCell(selectedPos);
        cellPos.y = 0;
        
        // check if we didnt move positions
        if (lastPos != cellPos)
        {
            buildingState.UpdateState(cellPos);
            
            lastPos = cellPos;
        }
    }

    private void StopPlacement()
    {
        if(gridVisualization != null)
            gridVisualization.SetActive(false);
        gridInput.OnClick -= PlaceStructure;
        gridInput.OnExit -= StopPlacement;
        
        lastPos = Vector3Int.zero;
        
        if(buildingState == null)
            return;
        
        buildingState.EndState();
        buildingState = null;
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        
        if(gridVisualization != null)
            gridVisualization?.SetActive(true);
        
        buildingState = new GridPlacementState(id, grid, gridPreview, dataBase, gridObjectsData, objectPlacer);
        
        gridInput.OnClick += PlaceStructure;
        gridInput.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        
        if(gridVisualization != null)
            gridVisualization?.SetActive(true);
        
        buildingState = new GridRemovingState(grid, gridPreview, gridObjectsData, objectPlacer);
        
        gridInput.OnClick += PlaceStructure;
        gridInput.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        Vector3 selectedPos = gridInput.GetSelectedMapPosition();

        buildingState.OnAction(selectedPos);
    }

    public void SetObligatoryOccupiedSpaces(CustomBoolMatrix placementMatrix)
    {
        int rows = placementMatrix.rows;
        int colums = placementMatrix.columns;

        gridObjectsData.AddObject(new Vector2Int(-(rows / 2 + 1), -(colums / 2 + 1)), placementMatrix, -1, -1);

        SetBordersAsOccupiedSpaces(rows, colums);
    }

    private void SetBordersAsOccupiedSpaces(int rows, int colums)
    {
        // >
        Vector2Int pos = new Vector2Int(-(rows / 2 + 1), -(colums / 2 + 1));
        Vector2Int spaces = new Vector2Int(1, colums + 1);
        gridObjectsData.AddObject(pos, spaces, -1, -1);

        // ^
        pos = new Vector2Int(rows / 2 + 1, -(colums / 2 + 1));
        spaces = new Vector2Int(rows + 1, 1);
        gridObjectsData.AddObject(pos, spaces, -1, -1);

        // <
        pos = new Vector2Int(rows / 2 + 1, colums / 2 + 1);
        spaces = new Vector2Int(-(rows + 1), 1);
        gridObjectsData.AddObject(pos, spaces, -1, -1);

        // V
        pos = new Vector2Int(-(rows / 2 + 1), colums / 2 + 1);
        spaces = new Vector2Int(1, -(colums + 1));
        gridObjectsData.AddObject(pos, spaces, -1, -1);
    }
}