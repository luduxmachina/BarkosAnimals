using System;
using System.Data;
using UnityEngine;

public class WorldGridInput : MonoBehaviour, IGridInput
{
    public event Action OnClick, OnExit;

    [SerializeField]
    private LayerMask groundLayerMask;
    
    [SerializeField]
    private GridPlacementManager gridPlacementManager;

    [SerializeField]
    private B_WorldPlaceableObjectsSO bDataBase;
    [SerializeField]
    private C_WorldPlaceableObjectsSO cDataBase;

    // private BasePlaceableObjectsSO<PlaceableObjectDataBase> actualDB;

    private int rows = 0;
    private int colums = 0;
    private int actualObjId = -1;
    private float gridSize = 1;

    public Vector3 GetSelectedMapPosition()
    {
        float r = rows / 2;
        float c = colums / 2;

        // var aux = actualDB.GetPlaceableObjects();
        // int objRows = aux[actualObjId].OcupiedSpace.GetRows();
        // int objColums = aux[actualObjId].OcupiedSpace.GetColums();
        
        return new Vector3(
            UnityEngine.Random.Range(-r * gridSize, r * gridSize),
            0f,
            UnityEngine.Random.Range(-c * gridSize, c * gridSize)
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

    public void PlaceBObjOfID(int id)
    {
        actualObjId = id;
        gridPlacementManager.StartPlacement(id, bDataBase);
        OnClick?.Invoke();
    }

    public void PlaceBObjOfRandomID()
    {
        int id = UnityEngine.Random.Range(0, bDataBase.GetPlaceableObjects().Count);
        PlaceBObjOfID(id);
    }

    public void PlaceCObjOfID(int id)
    {
        actualObjId = id;
        gridPlacementManager.StartPlacement(id, cDataBase);
        OnClick?.Invoke();
    }

    public void PlaceCObjOfRandomID()
    {
        int id = UnityEngine.Random.Range(0, cDataBase.GetPlaceableObjects().Count);
        PlaceCObjOfID(id);
    }

    public void ConfigureInput(CustomBoolMatrix boolMatrix)
    {
        gridSize = gridPlacementManager.GetGridSize();

        rows = boolMatrix.rows;
        colums = boolMatrix.columns;
    }
}
