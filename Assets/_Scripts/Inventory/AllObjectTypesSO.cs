using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AllObjectTypesSO : ScriptableObject
{
    public List<ObjectTypes> itemsData = new();

    public ObjectTypes FindItem(ItemNames itemName)
    {
        foreach (var item in itemsData)
        {
            if (item.Name == itemName)
                return item;
        }
        
        return null;
    }
}