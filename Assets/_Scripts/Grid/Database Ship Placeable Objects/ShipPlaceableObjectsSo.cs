using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShipPlaceableObjectsSo : ScriptableObject, IPlaceableObjectsSO<ShipPlaceableObjectData>
{
    [field: SerializeField] public List<ShipPlaceableObjectData> PlaceableObjectData { get; set; }
    
    
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