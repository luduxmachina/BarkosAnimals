using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Grid))]
[RequireComponent(typeof(IGridInput))]
[RequireComponent(typeof(IObjectPlacer))]

// [RequireComponent(typeof(GridPreview))]

public class GridPlacementManager : MonoBehaviour
{
    [SerializeField] private ABasePlaceableObjectsSO dataBase;

    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private int numOfGrids = 1;

    public UnityEvent OnPlaceStructure = new();
    public UnityEvent OnRemoveStructure = new();
    

    private Grid grid;
    private IGridInput gridInput;
    private List<GridData> gridObjectsDatas;
    private GridPreview gridPreview;
    private IObjectPlacer objectPlacer;
    
    private Vector3Int lastPos = Vector3Int.zero;
    
    private IGridBuildingState buildingState;
    
    private bool isPlacing = false;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        gridInput = GetComponent<IGridInput>();
        objectPlacer = GetComponent<IObjectPlacer>();
        gridObjectsDatas = new List<GridData>();
        for (int i = 0; i < numOfGrids; i++)
        {
            gridObjectsDatas.Add(new GridData());
        }

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
        // cellPos.y = 0;
        
        // check if we didnt move positions
        if (lastPos != cellPos)
        {
            buildingState.UpdateState(cellPos);
            
            lastPos = cellPos;
        }
    }

    public void StopPlacement()
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
        
        buildingState = new GridPlacementState(id, grid, gridPreview, dataBase, gridObjectsDatas, objectPlacer);
        
        gridInput.OnClick += PlaceStructure;
        gridInput.OnExit += StopPlacement;
        
        isPlacing = true;
    }

    public void StartPlacement(int id, ABasePlaceableObjectsSO db)
    {
        var aux = dataBase;
        dataBase = db;

        StartPlacement(id);

        dataBase = aux;
    }

    public void StartRemoving()
    {
        StopPlacement();
        
        if(gridVisualization != null)
            gridVisualization?.SetActive(true);
        
        buildingState = new GridRemovingState(grid, gridPreview, gridObjectsDatas, objectPlacer);
        
        gridInput.OnClick += PlaceStructure;
        gridInput.OnExit += StopPlacement;
        
        isPlacing = false;
    }

    private void PlaceStructure()
    {
        Vector3 selectedPos = gridInput.GetSelectedMapPosition();

        bool placed = buildingState.OnAction(selectedPos);

        if (placed)
        {
            if (isPlacing)
            {
                OnPlaceStructure?.Invoke();
            }
            else
            {
                OnRemoveStructure?.Invoke();
            }
        }
    }

    public void SetObligatoryOccupiedSpaces(CustomBoolMatrix placementMatrix)
    {
        int rows = placementMatrix.rows;
        int colums = placementMatrix.columns;
        
        CustomBoolMatrix occupiedSpace = new CustomBoolMatrix(rows + 2, colums + 2);
        occupiedSpace.EnsureSize();

        for (int i = 0; i < occupiedSpace.GetRows(); i++)
        {
            for (int j = 0; j < occupiedSpace.GetColums(); j++)
            {
                if (i == 0 || j == 0 || i == occupiedSpace.GetRows() - 1 || j == occupiedSpace.GetColums() - 1)
                {
                    occupiedSpace.SetValue(i, j, true);
                }
                else
                {
                    occupiedSpace.SetValue(i, j, placementMatrix.GetValue(i - 1, j - 1));
                }
            }
        }

        // occupiedSpace.DebugMatrix();
        foreach (var gd in gridObjectsDatas)
        {
            gd.AddObject(new Vector2Int(-(rows / 2) - 1, -(colums / 2) - 1), occupiedSpace, -1, -1);
        }
    }

    public float GetGridSize()
    {
        return grid.cellSize.x;
    }
}