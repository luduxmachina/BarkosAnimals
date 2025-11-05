using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class B_WorldPlaceableObjectsSO : BasePlaceableObjectsSO<B_WolrdObjectData>
{
    private void OnValidate()
    {
        for (int i = 0; i < PlaceableObjectData.Count; i++)
        {
            var obj =  PlaceableObjectData[i];

            obj.ID = i;
            
            // Matrices de espacios ocupados
            if (obj.OcupiedSpace != null)
                obj.OcupiedSpace.EnsureSize();

            if (obj.C_OcupiedSpace != null)
                obj.C_OcupiedSpace.EnsureSize();

            if (obj.OcupiedSpace.GetRows() != obj.C_OcupiedSpace.GetRows())
            {
                obj.C_OcupiedSpace.rows = obj.OcupiedSpace.GetRows();
                obj.C_OcupiedSpace.EnsureSize();
            }

            if (obj.OcupiedSpace.GetColums() != obj.C_OcupiedSpace.GetColums())
            {
                obj.C_OcupiedSpace.columns = obj.OcupiedSpace.GetColums();
                obj.C_OcupiedSpace.EnsureSize();
            }
        }
    }
}

[Serializable]
public class B_WolrdObjectData : PlaceableObjectDataBase
{
    [field: SerializeField] public CustomBoolMatrix C_OcupiedSpace = new CustomBoolMatrix();
}
