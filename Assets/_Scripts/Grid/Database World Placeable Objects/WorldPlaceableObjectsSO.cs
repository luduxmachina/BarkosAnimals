using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldPlaceableObjectsSO", menuName = "Scriptable Objects/WorldPlaceableObjectsSO")]
public class WorldPlaceableObjectsSO : ScriptableObject
{
    public List<WolrdObjectData> worldObjectData;

    private void OnValidate()
    {
        foreach (var obj in worldObjectData)
        {
            // Matrices de espacios ocupados
            if (obj.B_OcupiedSpace != null)
                obj.B_OcupiedSpace.EnsureSize();

            if (obj.C_OcupiedSpace != null)
                obj.C_OcupiedSpace.EnsureSize();

            if (obj.B_OcupiedSpace.GetRows() != obj.C_OcupiedSpace.GetRows())
            {
                obj.C_OcupiedSpace.rows = obj.B_OcupiedSpace.GetRows();
                obj.C_OcupiedSpace.EnsureSize();
            }

            if (obj.B_OcupiedSpace.GetColums() != obj.C_OcupiedSpace.GetColums())
            {
                obj.C_OcupiedSpace.columns = obj.B_OcupiedSpace.GetColums();
                obj.C_OcupiedSpace.EnsureSize();
            }
        }
    }
}

[Serializable]
public class WolrdObjectData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }

    [field: SerializeField] public CustomBoolMatrix B_OcupiedSpace = new CustomBoolMatrix();
    [field: SerializeField] public CustomBoolMatrix C_OcupiedSpace = new CustomBoolMatrix();
}
