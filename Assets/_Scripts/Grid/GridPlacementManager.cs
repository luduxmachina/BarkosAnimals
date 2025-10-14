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
    
    private float rotation = 0f;

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
            buildingState.UpdateState(cellPos, rotation);
            
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

    public void StartPlacement(int id)
    {
        StopPlacement();
        
        gridVisualization.SetActive(true);
        
        buildingState = new GridPlacementState(id, grid, gridPreview, dataBase, gridObjectsData, objectPlacer);
        
        gridInput.OnClick += PlaceStructure;
        gridInput.onExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        
        gridVisualization.SetActive(true);
        
        buildingState = new GridRemovingState(grid, gridPreview, gridObjectsData, objectPlacer);
        
        gridInput.OnClick += PlaceStructure;
        gridInput.onExit += StopPlacement;
    }
    
    public void SetRotation(int id, float rotation)
    {
        this.rotation = rotation;
        
        // Visuals
        // if (id > 0)
        // {
        //     gridPreview.StartPreview(dataBase.objectData[id].Prefab, dataBase.objectData[id].Size, rotation);
        // }
        // else
        // {
        //     gridPreview.StartPreview(null, Vector2Int.one, rotation);
        // }
    }

    private void PlaceStructure()
    {
        Vector3 selectedPos = gridInput.GetSelectedMapPosition();

        buildingState.OnAction(selectedPos, rotation);
    }
}