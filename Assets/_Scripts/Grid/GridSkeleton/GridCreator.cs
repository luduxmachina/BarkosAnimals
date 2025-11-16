using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GridCreator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> grids;
    [SerializeField]
    private CustomBoolMatrix gridOcupiedSpaces = new CustomBoolMatrix();
    [SerializeField]
    private int cellSize = 1;

    public UnityEvent<CustomBoolMatrix> ConfigureGrid;
    public UnityEvent OnGridCreated;
    public UnityEvent OnDecorationPlacing;
    public UnityEvent OnNavMeshGenerating;
    public UnityEvent<Vector2> OnItemPlacing;

    private List<GridPlacementManager> placementManagers = new();

    private void Awake()
    {   
        // Vector3 scale = new Vector3(cellSize * wolrdGridOcupiedSpaces.columns, cellSize * wolrdGridOcupiedSpaces.rows, 1);
        // grid.transform.localScale = scale;

        foreach (var grid in grids)
        {
            grid.GetComponent<Grid>().cellSize = Vector3.one * cellSize;
            placementManagers.Add(grid.GetComponent<GridPlacementManager>());
        }
    }

    private void Start()
    {
        foreach (var placementManager in placementManagers)
        {
            placementManager.SetObligatoryOccupiedSpaces(gridOcupiedSpaces);
        }
        
        Vector2 worldDimensions = new Vector2(gridOcupiedSpaces.rows * cellSize, gridOcupiedSpaces.columns * cellSize);

        ConfigureGrid?.Invoke(gridOcupiedSpaces);
        OnGridCreated?.Invoke();
        OnDecorationPlacing?.Invoke();
        OnNavMeshGenerating?.Invoke();
        OnItemPlacing?.Invoke(worldDimensions);
    }
}
