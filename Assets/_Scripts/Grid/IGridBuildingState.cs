using UnityEngine;

public interface IGridBuildingState
{
    void EndState();
    void OnAction(Vector3 position, float orientation);
    void UpdateState(Vector3Int cellPos, float orientation);
}