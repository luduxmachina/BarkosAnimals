using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class AllObjectTypesSO : ScriptableObject
{
    public List<ObjectTypes> itemsData = new();

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Verifica duplicados por nombre
        var duplicates = itemsData
            .GroupBy(i => i.Name)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicates.Count > 0)
        {
            string duplicateNames = string.Join(", ", duplicates);
            Debug.LogWarning($"[AllObjectTypesSO] Hay objetos duplicados: {duplicateNames}", this);

            // Opcional: eliminar duplicados autom�ticamente
            // itemsData = itemsData.GroupBy(i => i.Name).Select(g => g.First()).ToList();

            // Tambi�n puedes marcar el asset como "dirty" si haces cambios:
            // EditorUtility.SetDirty(this);
        }
    }
#endif

    public ObjectTypes FindItem(ItemNames itemName)
    {
        foreach (var item in itemsData)
        {
            if (item.Name == itemName)
                return item;
        }
        
        throw new KeyNotFoundException($"Item with name {itemName} not found on the objects data base");
    }

    public bool ConteinsItem(ItemNames itemName)
    {
        foreach (var item in itemsData)
        {
            if (item.Name == itemName)
            {
                return true;
            }
        }
        
        return false;
    }

    public ItemType GetItemType(ItemNames itemName) => FindItem(itemName).Type;
    public GameObject GetObjectPrefab(ItemNames itemName) => FindItem(itemName).Prefab;
    public Sprite GetSprite(ItemNames itemName) => FindItem(itemName).ItemImage;
    public int GetObjectMaxStackSize(ItemNames itemName) => FindItem(itemName).MaxStackSize;
    public List<Restriction> GetRestrictions(ItemNames itemName) => FindItem(itemName).checkRestrictions;
}