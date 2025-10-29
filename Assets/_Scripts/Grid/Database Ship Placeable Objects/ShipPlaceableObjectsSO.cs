using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShipPlaceableObjectsSO : ScriptableObject
{
    public List<ShipObjectData> shipObjectData;

    private void OnValidate()
    {
        foreach (var obj in shipObjectData)
        {
            if (obj.OcupiedSpace != null)
                obj.OcupiedSpace.EnsureSize();
        }
    }
}

[Serializable]
public class ShipObjectData
{
    [field: SerializeField] public string Name{ get; private set; }
    [field: SerializeField] public int ID{ get; set; }
    // [field: SerializeField] public Vector2Int Size { get ; private set; } = Vector2Int.one;
    [field: SerializeField] public GameObject Prefab{ get; private set; }
    [field: SerializeField] public Sprite ImageUI{ get; private set; }
    [field: SerializeField] public CustomBoolMatrix OcupiedSpace = new CustomBoolMatrix(); // { get; private set; } = new CustomBoolMatrix();

    public Vector2Int Size
    {
        get
        {
            if (OcupiedSpace == null)
                return Vector2Int.zero;

            return new Vector2Int(OcupiedSpace.rows, OcupiedSpace.columns);
        }
    }
}