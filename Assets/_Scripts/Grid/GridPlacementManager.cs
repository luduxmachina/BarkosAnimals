using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridPlacementManager : MonoBehaviour
{
    public GameObject mouseIndicator, cellIndicator, prefabToPlace;


    [SerializeField] private PlaceableObjectsSO dataBase;
    private int selectedObjectIndex = -1;

    [SerializeField] private GameObject gridVisualization;
    
    [SerializeField]
    private Color CanBePlacedColor = Color.white;
    [SerializeField]
    private Color CanNotBePlacedColor = Color.red;

    private Grid grid;
    private GridInput gridInput;
    private GridData gridObjectsData;
    private Renderer previewRenderer;

    private HashSet<GameObject> placedObjects = new HashSet<GameObject>();

    private void Awake()
    {
        grid = GetComponent<Grid>();
        gridInput = GetComponent<GridInput>();
    }

    private void Start()
    {
        StopPlacement();

        gridObjectsData = new GridData();
        previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
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
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        worldCellPos = new Vector3(
            worldCellPos.x - grid.cellSize.x * 0.5f,
            cellIndicator.transform.position.y,
            worldCellPos.z - grid.cellSize.z * 0.5f
        );
        cellIndicator.transform.position = worldCellPos;
        // cellIndicator.transform.position = new Vector3
        // (
        //     cellPos.x,
        //     cellIndicator.transform.position.y,
        //     cellPos.z
        // );
        
        // Check if can be placed
        Vector2Int realCellPos = new Vector2Int(cellPos.x, cellPos.z);
        bool validPlace = CheckPlacementValidity(realCellPos, selectedObjectIndex);
        previewRenderer.material.color = validPlace ? CanBePlacedColor : CanNotBePlacedColor;
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;

        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        mouseIndicator.SetActive(false);
        gridInput.OnClick -= PlaceStructure;
        gridInput.onExit -= StopPlacement;
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
        cellIndicator.SetActive(true);
        mouseIndicator.SetActive(true);
        gridInput.OnClick += PlaceStructure;
        gridInput.onExit += StopPlacement;

        Vector2 objSize = dataBase.objectData[selectedObjectIndex].Size;
        cellIndicator.transform.localScale = new Vector3(objSize.x, 1, objSize.y);
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
            0, //cellIndicator.transform.position.y,
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