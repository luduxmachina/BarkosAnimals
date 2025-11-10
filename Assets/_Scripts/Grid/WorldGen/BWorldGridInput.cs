using System;
using System.Data;
using UnityEngine;

public class BWorldGridInput : MonoBehaviour, IGridInput
{
    public event Action OnClick, OnExit;

    [SerializeField]
    private LayerMask groundLayerMask;
    
    [SerializeField]
    private GridPlacementManager gridPlacementManager;

    [SerializeField]
    private B_WorldPlaceableObjectsSO dataBase;

    private int rows = 0;
    private int colums = 0;
    private int actualObjId = -1;
    private float gridSize = 1;

    public Vector3 GetSelectedMapPosition()
    {
        float r = rows / 2;
        float c = colums / 2;

        var aux = dataBase.GetPlaceableObjects();
        int objRows = aux[actualObjId].OcupiedSpace.GetRows();
        int objColums = aux[actualObjId].OcupiedSpace.GetColums();
        
        return new Vector3(
            UnityEngine.Random.Range(-r * gridSize, Mathf.Max(0, (r - objRows) * gridSize)),
            0f,
            UnityEngine.Random.Range(-c * gridSize, Mathf.Max(0, (c - objColums) * gridSize))
        );
    }

    public void StartPlacing(int id)
    {
        actualObjId = id;
        gridPlacementManager.StartPlacement(id);
    }

    public void StartRemoving()
    {
        throw new NotImplementedException();
    }

    // public void ReadyForStartPlacing()
    // {
    //     // TESTEO
    //     // PlaceObjOfID(0);
    //     PlaceObjOfID(2);
    // }

    public void PlaceObjOfID(int id)
    {
        StartPlacing(id);
        OnClick?.Invoke();
    }

    public void PlaceObjOfRandomID()
    {
        int id = UnityEngine.Random.Range(0, dataBase.GetPlaceableObjects().Count);
        PlaceObjOfID(id);
    }

    public void ConfigureInput(CustomBoolMatrix boolMatrix)
    {
        gridSize = gridPlacementManager.GetGridSize();

        rows = boolMatrix.rows;
        colums = boolMatrix.columns;
    }
}
