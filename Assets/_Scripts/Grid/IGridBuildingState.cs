using UnityEngine;

public interface IGridBuildingState
{
    void EndState();
    void OnAction(Vector3 position);
    void UpdateState(Vector3Int cellPos);
}