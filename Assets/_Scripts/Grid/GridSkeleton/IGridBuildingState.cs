using UnityEngine;

public interface IGridBuildingState
{
    void EndState();
    bool OnAction(Vector3 position);
    void UpdateState(Vector3Int cellPos);
}