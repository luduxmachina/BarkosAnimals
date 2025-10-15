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

            // Opcional: eliminar duplicados automáticamente
            // itemsData = itemsData.GroupBy(i => i.Name).Select(g => g.First()).ToList();

            // También puedes marcar el asset como "dirty" si haces cambios:
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

    public ItemType GetItemType(ItemNames itemName) => FindItem(itemName).Type;
    public GameObject GetObjectPrefab(ItemNames itemName) => FindItem(itemName).Prefab;
    public int GetObjectMaxStackSize(ItemNames itemName) => FindItem(itemName).MaxStackSize;
    public bool GetIsAnimal(ItemNames itemName) => FindItem(itemName).IsAnimal;
    public bool GetIsCarnivore(ItemNames itemName) => FindItem(itemName).IsCarnivore;
    public bool GetIsFood(ItemNames itemName) => FindItem(itemName).IsFood;
}