using BehaviourAPI.UnityToolkit;
using UnityEngine;
[SelectionGroup("PROPIOS")]
public class PathingActionTransformParent : PathingAction
{
    public Transform wayPointsParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        positions = new System.Collections.Generic.List<Vector3>();
        for (int i = 0; i < wayPointsParent.childCount; i++)
        {
            positions.Add(wayPointsParent.GetChild(i).position);
        }
        base.Start();
    }

}
