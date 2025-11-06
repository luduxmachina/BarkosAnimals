using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WorldGridCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject grid;
    [SerializeField]
    private CustomBoolMatrix wolrdGridOcupiedSpaces = new CustomBoolMatrix();
    [SerializeField]
    private int cellSize = 4;

    public UnityEvent OnWorldGridCreated;

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
        placementManager.SetObligatoryOccupiedSpaces(wolrdGridOcupiedSpaces);

        OnWorldGridCreated?.Invoke();
    }
}
