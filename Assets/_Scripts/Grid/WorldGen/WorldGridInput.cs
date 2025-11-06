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
        StartPlacing(0);
        OnClick?.Invoke();
        StartPlacing(1);
        OnClick?.Invoke();
    }
}
