using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShipPlaceableObjectsSO : BasePlaceableObjectsSO<ShipPlaceableObjectData>
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
public class ShipPlaceableObjectData : PlaceableObjectDataBase
{
    [field: SerializeField] public Sprite ImageUI { get; private set; }
}