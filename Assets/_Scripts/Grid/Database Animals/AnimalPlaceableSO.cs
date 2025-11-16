using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimalPlaceableSO: BasePlaceableObjectsSO<PlaceableObjectDataBase>
{
    private void Awake()
    {
        placeableType = PlaceableDatabaseType.Animal;
    }
    
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
