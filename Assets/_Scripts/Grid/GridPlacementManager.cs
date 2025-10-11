using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Grid))]
[RequireComponent(typeof(GridInput))]
[RequireComponent(typeof(GridPreview))]
[RequireComponent(typeof(ObjectPlacer))]
public class GridPlacementManager : MonoBehaviour
{
    [SerializeField] private PlaceableObjectsSO dataBase;

    [SerializeField] private GameObject gridVisualization;

    private Grid grid;
    private GridInput gridInput;
    private GridData gridObjectsData;
    private GridPreview gridPreview;
    private ObjectPlacer objectPlacer;
    
    private Vector3Int lastPos = Vector3Int.zero;
    
    private IGridBuildingState buildingState;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        gridInput = GetComponent<GridInput>();
        gridPreview = GetComponent<GridPreview>();
        objectPlacer = GetComponent<ObjectPlacer>();
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
        if(buildingState == null)
            return;
        
        gridVisualization.SetActive(false);
        gridInput.OnClick -= PlaceStructure;
        gridInput.onExit -= StopPlacement;
        
        lastPos = Vector3Int.zero;
        
        buildingState.EndState();
        buildingState = null;
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        
        gridVisualization.SetActive(true);
        
        buildingState = new GridPlacementState(ID, grid, gridPreview, dataBase, gridObjectsData, objectPlacer);
        
        gridInput.OnClick += PlaceStructure;
        gridInput.onExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        Vector3 selectedPos = gridInput.GetSelectedMapPosition();

        buildingState.OnAction(selectedPos);
    }
}