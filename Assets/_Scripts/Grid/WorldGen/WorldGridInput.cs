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
        return Vector3.zero;
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
        Debug.Log("ReadyForStartPlacing");
        StartPlacing(0);
        OnClick?.Invoke();
    }
}
