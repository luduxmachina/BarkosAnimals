using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GridCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject grid;
    [SerializeField]
    private CustomBoolMatrix gridOcupiedSpaces = new CustomBoolMatrix();
    [SerializeField]
    private int cellSize = 1;

    public UnityEvent<CustomBoolMatrix> ConfigureGrid;
    public UnityEvent OnGridCreated;
    

    private GridPlacementManager placementManager;

    private void Awake()
    {   
        // Vector3 scale = new Vector3(cellSize * wolrdGridOcupiedSpaces.columns, cellSize * wolrdGridOcupiedSpaces.rows, 1);
        // grid.transform.localScale = scale;

        grid.GetComponent<Grid>().cellSize = Vector3.one * cellSize; // new Vector3(cellSize, 1, cellSize);
        placementManager = grid.GetComponent<GridPlacementManager>();
    }

    private void Start()
    {
        placementManager.SetObligatoryOccupiedSpaces(gridOcupiedSpaces);

        ConfigureGrid?.Invoke(gridOcupiedSpaces);
        OnGridCreated?.Invoke();
    }
}
