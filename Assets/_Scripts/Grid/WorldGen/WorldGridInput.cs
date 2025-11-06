using System;
using UnityEngine;

public class WorldGridInput : MonoBehaviour, IGridInput
{
    public event Action OnClick, OnExit;

    [SerializeField]
    private LayerMask groundLayerMask;
    
    [SerializeField]
    private GridPlacementManager gridPlacementManager;

    public Vector3 GetSelectedMapPosition()
    {
        // TESTEO
        return new Vector3(0f, 0f, 0f);
    }

    public void StartPlacing(int id)
    {
        gridPlacementManager.StartPlacement(id);
    }

    public void StartRemoving()
    {
        throw new NotImplementedException();
    }

    public void ReadyForStartPlacing()
    {
        // TESTEO
        // PlaceObjOfID(0);
        PlaceObjOfID(2);
    }

    private void PlaceObjOfID(int id)
    {
        StartPlacing(id);
        OnClick?.Invoke();
    }
}
