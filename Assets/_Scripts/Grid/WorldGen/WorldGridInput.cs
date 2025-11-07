using System;
using UnityEngine;

public class WorldGridInput : MonoBehaviour, IGridInput
{
    public event Action OnClick, OnExit;

    [SerializeField]
    private LayerMask groundLayerMask;

    public Vector3 GetSelectedMapPosition()
    {
        throw new NotImplementedException();
    }

    public void StartPlacing(int id)
    {
        throw new NotImplementedException();
    }

    public void StartRemoving()
    {
        throw new NotImplementedException();
    }
}
