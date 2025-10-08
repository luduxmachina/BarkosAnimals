using System;
using UnityEngine;

public class GridPlacementManager : MonoBehaviour
{
    public GameObject mouseIndicator, cellIndicator, prefabToPlace;
    
    
    [SerializeField] 
    private PlaceableObjectsSO dataBase;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;
    
    private Grid grid;
    private GridInput gridInput;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        gridInput = GetComponent<GridInput>();
    }

    private void Update()
    {
        Vector3 selectedPos = gridInput.GetSelectedMapPosition();

        mouseIndicator.transform.position = selectedPos;
        
        Vector3Int cellPos = grid.WorldToCell(selectedPos);
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        
        Vector2 objSize= dataBase.objectData[selectedObjectIndex].Size;
        
        cellIndicator.transform.position = new Vector3
        (
            worldCellPos.x - 0.5f * (objSize.x - 1),
            cellIndicator.transform.position.y, 
            worldCellPos.z + 0.5f * (objSize.y - 1)
        );
    }
    
    private void Start()
    {
        StopPlacement();
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
        
        Vector2 objSize= dataBase.objectData[selectedObjectIndex].Size;
        cellIndicator.transform.localScale = new Vector3(objSize.x, objSize.y, 1);
    }

    private void PlaceStructure()
    {
        Vector3 selectedPos = gridInput.GetSelectedMapPosition();
        
        Vector3Int cellPos = grid.WorldToCell(selectedPos);
        Vector3 worldCellPos = grid.GetCellCenterWorld(cellPos);
        
        GameObject newObj = Instantiate(dataBase.objectData[selectedObjectIndex].Prefab);
        newObj.transform.position = new Vector3
        (
            worldCellPos.x, 
            cellIndicator.transform.position.y, 
            worldCellPos.z
        );
        
        if(gridInput.GetPlacementInput())
            Instantiate(newObj, worldCellPos, Quaternion.identity);
    }
}