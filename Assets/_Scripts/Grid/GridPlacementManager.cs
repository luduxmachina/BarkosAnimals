using System;
using UnityEngine;

public class GridPlacementManager : MonoBehaviour
{
    public GameObject MouseIndicator, prefabToPlace;
    
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
        Vector3Int cellPos = grid.WorldToCell(selectedPos);
        MouseIndicator.transform.position = grid.GetCellCenterWorld(cellPos);
        
        if(gridInput.GetPlacementInput())
            Instantiate(prefabToPlace, MouseIndicator.transform.position, Quaternion.identity);
    }
}