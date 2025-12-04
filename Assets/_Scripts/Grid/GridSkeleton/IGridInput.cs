using System;
using UnityEngine;

public interface IGridInput
{
    event Action OnClick;
    event Action OnExit;

    Vector3 GetSelectedMapPosition();
    void StartPlacing(int id);
    void StopPlacing();
    void StartRemoving();
}