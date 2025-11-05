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
    [SerializeField] private ScriptableObject  DataBase;
    private IPlaceableObjectsSO<ShipPlaceableObjectData> dataBase => DataBase as IPlaceableObjectsSO<ShipPlaceableObjectData>;

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

        if (TryGetComponent(out GridPreview aux))
        {
            gridPreview = aux;
        }
    }

    private void Start()
    {
        StopPlacement();

        gridObjectsData = new GridData();
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
        
        gridVisualization.SetActive(true);
        
        buildingState = new GridPlacementState(id, grid, gridPreview, dataBase, gridObjectsData, objectPlacer);
        
        gridInput.OnClick += PlaceStructure;
        gridInput.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        
        gridVisualization.SetActive(true);
        
        buildingState = new GridRemovingState(grid, gridPreview, gridObjectsData, objectPlacer);
        
        gridInput.OnClick += PlaceStructure;
        gridInput.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        Vector3 selectedPos = gridInput.GetSelectedMapPosition();

        buildingState.OnAction(selectedPos);
    }
}