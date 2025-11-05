using System;
using UnityEngine;

[CreateAssetMenu]
public class C_WorldPlaceableObjectsSO : BasePlaceableObjectsSO<C_WolrdObjectData>
{
    private void OnValidate()
    {
        for (int i = 0; i < PlaceableObjectData.Count; i++)
        {
            var obj =  PlaceableObjectData[i];

            obj.ID = i;
            
            if (obj.OcupiedSpace != null)
                obj.OcupiedSpace.EnsureSize();
        }
    }
}

[Serializable]
public class C_WolrdObjectData : PlaceableObjectDataBase
{
}
