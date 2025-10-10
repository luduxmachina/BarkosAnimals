using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridPlacementManager : MonoBehaviour
{
    public GameObject mouseIndicator;


    [SerializeField] private PlaceableObjectsSO dataBase;
    private int selectedObjectIndex = -1;

    [SerializeField] private GameObject gridVisualization;

    private Grid grid;
    private GridInput gridInput;
    private GridData gridObjectsData;
    private GridPreview gridPreview;

    private HashSet<GameObject> placedObjects = new HashSet<GameObject>();

    private void Awake()
    {
        grid = GetComponent<Grid>();
        gridInput = GetComponent<GridInput>();
        gridPreview = GetComponent<GridPreview>();
    }

    private void Start()
    {
        StopPlacement();

        gridObjectsData = new GridData();
    }

    private void Update()
    {
        if (selectedObjectIndex < 0)
            return;
        
        // Visual Indicators
        MoveVisualIndicators();
    }

    private void MoveVisualIndicators()
    {
        Vector3 selectedPos = gridInput.GetSelectedMapPosition();
        mouseIndicator.transform.position = selectedPos;

        Vector3Int cellPos = grid.WorldToCell(selectedPos);
        cellPos.y = 0;
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        
        // Check if can be placed
        Vector2Int realCellPos = new Vector2Int(cellPos.x, cellPos.z);
        bool validPlace = CheckPlacementValidity(realCellPos, selectedObjectIndex);
        
        gridPreview.UpdatePosition(worldCellPos, validPlace, grid.cellSize.x);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;

        gridVisualization.SetActive(false);
        mouseIndicator.SetActive(false);
        gridInput.OnClick -= PlaceStructure;
        gridInput.onExit -= StopPlacement;

        gridPreview.StopPreview();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        
        selectedObjectIndex = dataBase.objectData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found: {ID}");
            return;
        }

        gridVisualization.SetActive(true);
        mouseIndicator.SetActive(true);
        gridInput.OnClick += PlaceStructure;
        gridInput.onExit += StopPlacement;

        gridPreview.StartPreview(dataBase.objectData[selectedObjectIndex].Prefab, dataBase.objectData[selectedObjectIndex].Size);
    }

    private void PlaceStructure()
    {
        Vector3 selectedPos = gridInput.GetSelectedMapPosition();

        Vector3Int cellPos = grid.WorldToCell(selectedPos);
        cellPos.y = 0;
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);

        // Check if can be placed
        Vector2Int realCellPos = new Vector2Int(cellPos.x, cellPos.z);
        bool validPlace = CheckPlacementValidity(realCellPos, selectedObjectIndex);
        if (!validPlace)
            return;

        // Place Object
        GameObject newObj = Instantiate(dataBase.objectData[selectedObjectIndex].Prefab);
        newObj.transform.position = new Vector3
        (
            worldCellPos.x,
            worldCellPos.y, // 0,
            worldCellPos.z
        );
        
        placedObjects.Add(newObj);
        GridData selectedGrid = GetSlelectedGrid(selectedObjectIndex); 
        selectedGrid.AddObject(realCellPos, dataBase.objectData[selectedObjectIndex].Size);
    }

    private bool CheckPlacementValidity(Vector2Int cellPos, int ObjectID)
    {
        GridData selectedGrid = GetSlelectedGrid(ObjectID); 

        return selectedGrid.CanPlaceObjectAt(cellPos, dataBase.objectData[selectedObjectIndex].Size);
    }

    private GridData GetSlelectedGrid(int objectID)
    {
        return gridObjectsData; // Can be selected multiple gridDatas in order to select floor or walls if needed
    }
}